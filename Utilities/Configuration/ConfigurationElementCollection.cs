using System.Configuration;
using System.Collections.Generic;

namespace Utilities.Configuration
{
    public class ConfigurationElementCollection<T> : ConfigurationElementCollection, IEnumerable<T> where T : ConfigurationHeaderElement, new()
    {
        List<T> _elements = new List<T>();

        protected override ConfigurationElement CreateNewElement()
        {
            T newElement = new T();
            _elements.Add(newElement);
            return newElement;
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return _elements.Find(e => e.Equals(element));
        }

        public List<T> GetElementsByPropertyValue(string Property, object Value)
        {
            List<T> elements = new List<T>();
            foreach (T element in this)
            {
                object prop_val = Reflection.Reflection.GetPropertyValue(element, Property);
                if (prop_val.Equals(Value))
                    elements.Add(element);
            }

            return elements;
        }

        public new IEnumerator<T> GetEnumerator()
        {
            return _elements.GetEnumerator();
        }
    }
}
