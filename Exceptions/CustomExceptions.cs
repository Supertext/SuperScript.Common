using System;
using System.Collections.ObjectModel;
using SuperScript.Emitters;

namespace SuperScript
{
	/// <summary>
	/// A base class for all exceptions within the SuperScript library.
	/// </summary>
	[Serializable]
	public class SuperScriptException : Exception
	{
		/// <summary>
		/// Contains specific details on the cause of the exception.
		/// </summary>
		public string ErrorMessage
		{
			get { return base.Message; }
		}

		/// <summary>
		/// A constructor for the <see cref="SuperScriptException"/> base class which permits an <see cref="ErrorMessage"/> to be specified.
		/// </summary>
		/// <param name="errorMessage">
		/// Contains specific details on the cause of the exception.
		/// </param>
		public SuperScriptException(string errorMessage)
			: base(errorMessage)
		{
		}

		/// <summary>
		/// A constructor for the <see cref="SuperScriptException"/> base class which permits an <see cref="ErrorMessage"/> to be specified
		/// along with an inner exception for more detailed debugging information.
		/// </summary>
		/// <param name="errorMessage">
		/// Contains specific details on the cause of the exception.
		/// </param>
		/// <param name="innerException">
		/// An <see cref="Exception"/> which will be exposed as the InnerException property.
		/// </param>
		public SuperScriptException(string errorMessage, Exception innerException)
			: base(errorMessage, innerException)
		{
		}
	}


	/// <summary>
	/// This exception is normally thrown when a required property has not been set.
	/// </summary>
	public class NotSpecifiedException : SuperScriptException
	{
		/// <summary>
		/// A constructor for the <see cref="NotSpecifiedException"/> base class which permits an <see cref="SuperScriptException.ErrorMessage"/> to be specified.
		/// </summary>
		/// <param name="errorMessage">
		/// Contains specific details on the cause of the exception.
		/// </param>
		public NotSpecifiedException(string errorMessage)
			: base(errorMessage)
		{
		}

		/// <summary>
		/// A constructor for the <see cref="NotSpecifiedException"/> base class which permits an <see cref="SuperScriptException.ErrorMessage"/> to be specified
		/// along with an inner exception for more detailed debugging information.
		/// </summary>
		/// <param name="errorMessage">
		/// Contains specific details on the cause of the exception.
		/// </param>
		/// <param name="innerException">
		/// An <see cref="Exception"/> which will be exposed as the InnerException property.
		/// </param>
		public NotSpecifiedException(string errorMessage, Exception innerException)
			: base(errorMessage, innerException)
		{
		}
	}


	/// <summary>
	/// <para>An <see cref="Exception"/> indicating that the <see cref="Collection{DeclarationBase}"/> has not been instantiated.</para>
	/// <para>By default, this should be instantiated in the <see cref="HttpInitialiser"/> handler, though it can be set anywhere.</para>
	/// </summary>
	public class CollectionNotInstantiatedException : SuperScriptException
	{
		/// <summary>
        /// Constructor for <see cref="CollectionNotInstantiatedException"/> which allows an exception-specific message to be relayed to the developer.
		/// </summary>
        public CollectionNotInstantiatedException(string message)
			: base(message)
		{
		}


		/// <summary>
		/// Parameterless constructor for the <see cref="CollectionNotInstantiatedException"/> <see cref="Exception"/>.
		/// </summary>
		public CollectionNotInstantiatedException()
			: base("The Collection (ICollection<DeclarationBase>) property has not been instantiated This should be done for each HTTP request.")
		{
		}
	}


	/// <summary>
	/// An <see cref="Exception"/> indicating that a configurable property (i.e., a property which is set in the 
	/// web.config) has not been set.
	/// </summary>
	public class ConfigurablePropertyNotSpecifiedException : NotSpecifiedException
	{
		/// <summary>
		/// Constructor for <see cref="ConfigurablePropertyNotSpecifiedException"/> which allows a property name
		/// to be specified to aid debugging or suggestions.
		/// </summary>
		/// <param name="propertyName">
		///     The name of the property which has not been set. This name will be displayed in an informative <see cref="Exception.Message"/>.
		/// </param>
		public ConfigurablePropertyNotSpecifiedException(string propertyName)
			: base("A property which must be declared in the web.config file is missing: " + propertyName)
		{
		}
	}


	/// <summary>
	/// An <see cref="Exception"/> indicating that a configurable property (i.e., a property which is set in the 
	/// web.config) has been set incorrectly.
	/// </summary>
	public class ConfigurationException : SuperScriptException
	{
		/// <summary>
		/// Constructor for <see cref="ConfigurationException"/> which allows an exception-specific message to be relayed to the developer.
		/// </summary>
		/// <param name="message">An informative message explaining the cause of this exception.</param>
		public ConfigurationException(string message) : base(message)
		{
		}
	}


	/// <summary>
	/// An <see cref="Exception"/> indicating that an error exists within the configuration for the default Emitter.
	/// </summary>
	public class DefaultEmitterException : ConfigurationException
	{
		/// <summary>
		/// Constructor for <see cref="DefaultEmitterException"/> which allows an exception-specific message to be relayed to the developer.
		/// </summary>
		/// <param name="message">An informative message explaining the cause of this exception.</param>
		public DefaultEmitterException(string message)
			: base(message)
		{
		}
	}


	/// <summary>
	/// An <see cref="Exception"/> indicating that a configurable property (i.e., a property which is set in the web.config)
	/// for an &lt;emitter&gt; element has been set incorrectly.
	/// </summary>
	public class EmitterConfigurationException : ConfigurationException
	{
		/// <summary>
		/// Constructor for <see cref="EmitterConfigurationException"/> which allows an exception-specific message to be relayed to the developer.
		/// </summary>
		/// <param name="message">An informative message explaining the cause of this exception.</param>
		public EmitterConfigurationException(string message)
			: base(message)
		{
		}
	}


	/// <summary>
	/// An <see cref="Exception"/> indicating that no instances of <see cref="IEmitter"/> have been configured.
	/// </summary>
	public class NoEmittersConfiguredException : SuperScriptException
	{
		/// <summary>
		/// Constructor for <see cref="NoEmittersConfiguredException"/> which allows an exception-specific message to be relayed to the developer.
		/// </summary>
		public NoEmittersConfiguredException(string message)
			: base(message)
		{
		}


		/// <summary>
		/// Constructor for <see cref="NoEmittersConfiguredException"/> which allows an exception-specific message to be relayed to the developer.
		/// </summary>
		public NoEmittersConfiguredException() : base("No instance of IEmitter has been configured and added to Declarations.Emitters.")
		{
		}
	}


	/// <summary>
	/// An <see cref="Exception"/> indicating that the specified property could not be found.
	/// </summary>
	public class SpecifiedPropertyNotFoundException : SuperScriptException
	{
		private const string Msg = "The specified property ({0}) could not be found on the host object.";

		/// <summary>
		/// Constructor for <see cref="SpecifiedPropertyNotFoundException"/> which allows the specifying of the name of the missing property.
		/// </summary>
		public SpecifiedPropertyNotFoundException(string propertyName) : base(String.Format(Msg, propertyName))
		{
		}


		/// <summary>
		/// Constructor for <see cref="SpecifiedPropertyNotFoundException"/> which allows an exception-specific message to be relayed to the developer.
		/// </summary>
		public SpecifiedPropertyNotFoundException()
			: base("The specified property could not be found on the host object.")
		{
		}
	}
}