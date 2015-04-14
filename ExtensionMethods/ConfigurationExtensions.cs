using SuperScript.Configuration;
using SuperScript.Emitters;
using SuperScript.Modifiers;
using SuperScript.Modifiers.Converters;
using SuperScript.Modifiers.Post;
using SuperScript.Modifiers.Pre;
using SuperScript.Modifiers.Writers;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace SuperScript.ExtensionMethods
{
    /// <summary>
    /// Contains extension methods which can be invoked upon the classes which are implemented in the web.config &lt;superScript&gt; section.
    /// </summary>
    public static class ConfigurationExtensions
	{
	    /// <summary>
	    /// Enumerates the <see cref="PropertyCollection"/> and populates the properties specified therein on the specified <see cref="host"/> object.
	    /// </summary>
	    /// <param name="propertyElmnts">A <see cref="PropertyCollection"/> object containing a collection of <see cref="PropertyElement"/> objects.</param>
	    /// <param name="host">The object to which the value of each <see cref="PropertyElement"/> object should be transferred to. </param>
	    public static void AssignProperties(this PropertyCollection propertyElmnts, object host)
	    {
		    if (propertyElmnts == null || propertyElmnts.Count == 0)
		    {
			    return;
		    }

		    foreach (PropertyElement propertyElmnt in propertyElmnts)
		    {
			    // a Type may be specified on the <property> element for the following reasons:
			    // - to assign a derived type to the specified property
			    // - if no value is specified on the <property> element then a new instance (using the default constructor)
			    //   will be created and assigned to the specified property. This branch will throw an exception if the 
			    //   specified type is a value type or does not have a public default constructor.

			    if (propertyElmnt.Type != null)
			    {
					// enums have to be handled differently
				    if (propertyElmnt.Type.IsEnum)
				    {
					    if (!Enum.IsDefined(propertyElmnt.Type, propertyElmnt.Value))
					    {
						    throw new SpecifiedPropertyNotFoundException(propertyElmnt.Name);
					    }

					    var enumInfo = host.GetType().GetProperty(propertyElmnt.Name);
					    if (enumInfo == null)
					    {
						    if (propertyElmnt.ExceptionIfMissing)
						    {
							    throw new SpecifiedPropertyNotFoundException(propertyElmnt.Name);
						    }

						    continue;
					    }

					    enumInfo.SetValue(host, Enum.Parse(propertyElmnt.Type, propertyElmnt.Value));

						continue;
				    }


				    // in the following call, passing customProperty.Type might be more secure, but cannot be done in case
				    // the target property has been specified using an interface.
				    var propInfo = host.GetType().GetProperty(propertyElmnt.Name, propertyElmnt.Type);
				    if (propInfo == null)
				    {
					    if (propertyElmnt.ExceptionIfMissing)
					    {
						    throw new SpecifiedPropertyNotFoundException(propertyElmnt.Name);
					    }

					    continue;
				    }

				    // if no value has been specified then assume that the intention is to assign a new instance of the
				    // specified type to the specified property.
				    if (String.IsNullOrWhiteSpace(propertyElmnt.Value) && !propertyElmnt.Type.IsValueType)
				    {
					    // check that the specified type has a public default (parameterless) constructor
					    if (propertyElmnt.Type.GetConstructor(Type.EmptyTypes) != null)
					    {
						    propInfo.SetValue(host,
						                      Activator.CreateInstance(propertyElmnt.Type),
						                      null);
					    }
				    }
				    else
				    {
					    // a Type and a value have been specified

					    // if the Type is System.TimeSpan then this needs to be parsed in a specific manner
					    if (propertyElmnt.Type == typeof (TimeSpan))
					    {
						    TimeSpan ts;
						    TimeSpan.TryParse(propertyElmnt.Value, out ts);
						    propInfo.SetValue(host,
						                      ts,
						                      null);
					    }
					    else
					    {
						    propInfo.SetValue(host,
						                      Convert.ChangeType(propertyElmnt.Value, propertyElmnt.Type),
						                      null);
					    }
				    }
			    }
			    else
			    {
				    var propInfo = host.GetType().GetProperty(propertyElmnt.Name);
				    if (propInfo == null)
				    {
					    if (propertyElmnt.ExceptionIfMissing)
					    {
						    throw new SpecifiedPropertyNotFoundException(propertyElmnt.Name);
					    }

					    continue;
				    }

				    propInfo.SetValue(host,
				                      Convert.ChangeType(propertyElmnt.Value, propInfo.PropertyType),
				                      null);
			    }
		    }
	    }


        /// <summary>
        /// Compares the specified <see cref="EmitMode"/> enumeration with the current context.
        /// </summary>
        /// <param name="emitMode">Determines for which modes (i.e., debug, live, or forced on or off) a property should be emitted.</param>
        /// <returns><c>True</c> if the current context covers the specified <see cref="EmitMode"/>.</returns>
        public static bool IsCurrentlyEmittable(this EmitMode emitMode)
        {
            if (emitMode == EmitMode.Always)
            {
                return true;
            }

            if (emitMode == EmitMode.Never)
            {
                return false;
            }

            if (Settings.IsDebuggingEnabled)
            {
                return emitMode == EmitMode.DebugOnly;
            }

            return emitMode == EmitMode.LiveOnly;
        }


        /// <summary>
		/// Determines whether the specified <see cref="ModifierBase"/> should be implemented in the current emitting context.
		/// </summary>
		/// <param name="modifier">An instance of <see cref="ModifierBase"/> whose <see cref="EmitMode"/> property will be checked against the current emitting context.</param>
		/// <returns><c>true</c> if the specified instance of <see cref="ModifierBase"/> should be emitted in the current emitting context.</returns>
		public static bool ShouldEmitForCurrentContext(this ModifierBase modifier)
	    {
	        return modifier.EmitMode.IsCurrentlyEmittable();
	    }


        /// <summary>
        /// Converts the specified <see cref="AttributesCollection"/> into an <see cref="ICollection{KeyValuePair}"/>.
        /// </summary>
        public static ICollection<KeyValuePair<string, string>> ToAttributeCollection(this AttributesCollection attributeElmnts)
        {
            var attrs = new Collection<KeyValuePair<string, string>>();

            foreach (AttributeElement attrElmnt in attributeElmnts)
            {
                attrs.Add(new KeyValuePair<string, string>(attrElmnt.Name, attrElmnt.Value));
            }

            return attrs;
        }


		/// <summary>
		/// Creates an instance of <see cref="EmitterBundle"/> from the specified <see cref="EmitterBundleElement"/> object.
		/// </summary>
		/// <exception cref="ConfigurationException">Thrown when a static or abstract type is referenced for the custom object.</exception>
	    public static EmitterBundle ToEmitterBundle(this EmitterBundleElement bundledEmitterElmnt)
	    {
		    // create the instance...
		    var instance = new EmitterBundle(bundledEmitterElmnt.Key);

		    // instantiate the CustomObject, if declared
		    if (bundledEmitterElmnt.CustomObject != null && bundledEmitterElmnt.CustomObject.Type != null)
		    {
			    var coType = bundledEmitterElmnt.CustomObject.Type;

			    // rule out abstract and static classes
			    // - reason: static classes will cause problems when we try to call static properties on the arguments.CustomObject property.
			    if (coType.IsAbstract)
			    {
				    throw new ConfigurationException("Static or abstract types are not permitted for the custom object.");
			    }

			    var customObject = Activator.CreateInstance(coType);

			    // if the developer has configured custom property values in the config then set them here
			    bundledEmitterElmnt.CustomObject.CustomProperties.AssignProperties(customObject);

				instance.CustomObject = customObject;
			}

		    var bundledKeys = new List<string>(bundledEmitterElmnt.BundleKeys.Count);
		    bundledKeys.AddRange(bundledEmitterElmnt.BundleKeys.Cast<string>());
		    instance.EmitterKeys = bundledKeys;

		    // instantiate the collection processors

		    instance.PostModifiers = bundledEmitterElmnt.PostModifiers.ToModifiers<CollectionPostModifier>();

		    instance.HtmlWriter = bundledEmitterElmnt.Writers.ToModifier<HtmlWriter>(required: true);

		    return instance;
	    }


	    /// <summary>
	    /// Creates an <see cref="ICollection{EmitterBundle}"/> containing the instances of <see cref="EmitterBundle"/> specified by the <see cref="EmitterBundlesCollection"/>.
	    /// </summary>
	    public static IList<EmitterBundle> ToEmitterBundles(this EmitterBundlesCollection bundledEmitterElmnts)
	    {
		    var bundledEmitters = new Collection<EmitterBundle>();

		    foreach (EmitterBundleElement bundledEmitterElmnt in bundledEmitterElmnts)
		    {
			    // check that no existing BundledEmitters have the same key as we're about to assign
			    if (bundledEmitters.Any(e => e.Key == bundledEmitterElmnt.Key))
			    {
				    throw new EmitterConfigurationException("Multiple <emitter> elements have been declared with the same Key (" + bundledEmitterElmnt.Key + ").");
			    }

			    bundledEmitters.Add(bundledEmitterElmnt.ToEmitterBundle());
		    }

		    return bundledEmitters;
	    }


	    /// <summary>
        /// Creates an instance of <see cref="IEmitter"/> from the specified <see cref="EmitterElement"/> object.
        /// </summary>
        /// <exception cref="ConfigurationException">Thrown when a static or abstract type is referenced for the custom object.</exception>
        public static IEmitter ToEmitter(this EmitterElement emitterElmnt)
        {
            // create the instance...
            var instance = emitterElmnt.ToInstance<IEmitter>();

            instance.IsDefault = emitterElmnt.IsDefault;
            instance.Key = emitterElmnt.Key;

            // instantiate the CustomObject, if declared
	        if (emitterElmnt.CustomObject != null && emitterElmnt.CustomObject.Type != null)
	        {
		        var coType = emitterElmnt.CustomObject.Type;

		        // rule out abstract and static classes
		        // - reason: static classes will cause problems when we try to call static properties on the arguments.CustomObject property.
		        if (coType.IsAbstract)
		        {
			        throw new ConfigurationException("Static or abstract types are not permitted for the custom object.");
		        }

		        var customObject = Activator.CreateInstance(coType);

		        // if the developer has configured custom property values in the config then set them here
		        emitterElmnt.CustomObject.CustomProperties.AssignProperties(customObject);

		        instance.CustomObject = customObject;
	        }

	        // instantiate the collection processors

            instance.PreModifiers = emitterElmnt.PreModifiers.ToModifiers<CollectionPreModifier>();

	        instance.Converter = emitterElmnt.Converters.ToModifier<CollectionConverter>(required: true);

            instance.PostModifiers = emitterElmnt.PostModifiers.ToModifiers<CollectionPostModifier>();

	        instance.HtmlWriter = emitterElmnt.Writers.ToModifier<HtmlWriter>();

            return instance;
        }


        /// <summary>
        /// Creates an <see cref="IList{IEmitter}"/> containing instances of the Emitters specified by the <see cref="EmittersCollection"/>.
        /// </summary>
        public static IList<IEmitter> ToEmitterCollection(this EmittersCollection emitterElmnts)
        {
            var emitters = new Collection<IEmitter>();

            foreach (var emitterElmnt in from EmitterElement emitterElement in emitterElmnts
                                    select emitterElement.ToEmitter())
            {
                // should we check if any other Emitters have been set to isDefault=true
                if (emitterElmnt.IsDefault && emitters.Any(e => e.IsDefault))
                {
                    throw new EmitterConfigurationException("Multiple <emitter> elements have been declared with 'isDefault' set to TRUE.");
                }

                // check that no existing emitters have the same key as we're about to assign
                if (emitters.Any(e => e.Key == emitterElmnt.Key))
                {
                    throw new EmitterConfigurationException("Multiple <emitter> elements have been declared with the same Key (" + emitterElmnt.Key + ").");
                }

                emitters.Add(emitterElmnt);
            }

            return emitters;
        }


	    /// <summary>
	    /// Returns the first instance of T which is eligible for the context emit mode (i.e., <see cref="EmitMode.Always"/>, <see cref="EmitMode.DebugOnly"/>, etc.).
	    /// </summary>
		/// <typeparam name="T">A type derived from <see cref="ModifierBase"/>.</typeparam>
		/// <param name="modifierElmnts">Contains a collection of instances of <see cref="ModifierBase"/>.</param>
	    /// <param name="required">Indicates whether this method must return a <see cref="ModifierBase"/> or whether these are optional.</param>
	    /// <exception cref="ConfigurationException">Thrown if multiple declarations have been made for the context emit mode.</exception>
	    /// <exception cref="ConfigurablePropertyNotSpecifiedException">Thrown if no declarations have been made for the context emit mode.</exception>
	    public static T ToModifier<T>(this ModifiersCollection modifierElmnts, bool required = false) where T : ModifierBase
	    {
		    T instanceToBeUsed = null;

		    foreach (ModifierElement modifierElmnt in modifierElmnts)
		    {
			    var modInstance = modifierElmnt.ToInstance<T>();
			    modInstance.EmitMode = modifierElmnt.EmitMode;
			    if (!modInstance.ShouldEmitForCurrentContext())
			    {
				    continue;
			    }

			    if (instanceToBeUsed != null)
			    {
				    throw new ConfigurationException("Only one instance of HtmlWriter (in a <writer> element) may be specified per context mode.");
			    }

			    modifierElmnt.ModifierProperties.AssignProperties(modInstance);

			    var bundled = modInstance as IUseWhenBundled;
			    if (bundled != null)
			    {
				    bundled.UseWhenBundled = modifierElmnt.UseWhenBundled;
				    instanceToBeUsed = (T) bundled;
			    }
			    else
			    {
				    instanceToBeUsed = modInstance;
			    }
		    }

		    if (required && instanceToBeUsed == null)
		    {
			    throw new ConfigurablePropertyNotSpecifiedException("writer");
		    }

		    return instanceToBeUsed;
	    }


	    /// <summary>
	    /// Creates an <see cref="ICollection{T}"/> containing instances of objects specified in each <see cref="ModifierElement"/>.
	    /// </summary>
	    /// <typeparam name="T">A type derived from <see cref="ModifierBase"/>.</typeparam>
	    /// <param name="modifierElmnts">Contains a collection of instances of <see cref="ModifierBase"/>.</param>
	    public static ICollection<T> ToModifiers<T>(this ModifiersCollection modifierElmnts) where T : ModifierBase
	    {
		    var instances = new Collection<T>();

		    foreach (ModifierElement modifierElmnt in modifierElmnts)
		    {
			    var modInstance = modifierElmnt.ToInstance<T>();
			    modInstance.EmitMode = modifierElmnt.EmitMode;
			    if (!modInstance.ShouldEmitForCurrentContext())
			    {
				    continue;
			    }

				modifierElmnt.ModifierProperties.AssignProperties(modInstance);

				var bundled = modInstance as IUseWhenBundled;
				if (bundled != null)
				{
					bundled.UseWhenBundled = modifierElmnt.UseWhenBundled;
					modInstance = (T) bundled;
				}

			    instances.Add(modInstance);
		    }

		    return instances;
	    }


	    /// <summary>
	    /// Returns the <see cref="Type"/> specified in the <see cref="IAssemblyElement"/>.
	    /// </summary>
	    public static T ToInstance<T>(this IAssemblyElement element)
	    {
		    return (T) Activator.CreateInstance(element.Type);
	    }
	}
}