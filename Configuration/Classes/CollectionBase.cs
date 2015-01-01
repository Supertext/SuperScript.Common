using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;

namespace SuperScript.Configuration
{
    /// <summary>
    ///     Generic implementation of ConfigurationElementCollection.
    /// </summary>
    [ConfigurationCollection(typeof (ConfigurationElement))]
    public class ConfigurationElementCollection<TElement> : ConfigurationElementCollection, IEnumerable<TElement> where TElement : ConfigurationElement,
                                                                                                                      IConfigurationCollectionItem,
                                                                                                                      new()
    {
        private readonly ConfigurationElementCollectionType _collectionType;
        private readonly string _elementName;

        public ConfigurationElementCollection()
        {

        }

        protected ConfigurationElementCollection(string elementName, ConfigurationElementCollectionType collectionType = ConfigurationElementCollectionType.AddRemoveClearMap)
        {
            _elementName = elementName;
            _collectionType = collectionType;
        }

        protected override string ElementName
        {
            get { return _elementName ?? ""; }
        }

        public override ConfigurationElementCollectionType CollectionType
        {
            get { return _collectionType; }
        }

        public TElement this[int index]
        {
            get { return (TElement) BaseGet(index); }
        }

        public IEnumerator<TElement> GetEnumerator()
        {
            foreach (var element in ((IEnumerable) this))
            {
                yield return (TElement) element;
            }
        }

        protected override bool IsElementName(string elementName)
        {
            if (_elementName == null)
                return base.IsElementName(elementName);

            return StringComparer.Ordinal.Equals(_elementName, elementName);
        }

        protected override ConfigurationElement CreateNewElement()
        {
            return new TElement();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((TElement) element).GetElementKey();
        }

        public TElement GetElement(object key)
        {
            return (TElement) BaseGet(key);
        }
    }

    public interface IConfigurationCollectionItem
    {
        object GetElementKey();
    }
}