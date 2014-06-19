using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.Collections
{
    public class ParameterValueEventArgs : EventArgs
    {
        public string Name { get; private set; }

        public string OldValue { get; private set; }

        public string NewValue { get; private set; }

        public ParameterValueEventArgs(string name, string oldValue, string newValue)
        {
            Name = name;
            OldValue = oldValue;
            NewValue = newValue;
        }
    }

    public class ParameterValueDictionary : IEnumerable<KeyValuePair<string, string>>
    {
        public event EventHandler<ParameterValueEventArgs> ValueAdded;

        public event EventHandler<ParameterValueEventArgs> ValueRemoved;

        public event EventHandler<ParameterValueEventArgs> ValueChanged;

        private Dictionary<string, string> _dictionary;

        public ParameterValueDictionary()
        {
            _dictionary = new Dictionary<string, string>();
        }

        public ParameterValueDictionary(IEqualityComparer<string> comparer)
        {
            _dictionary = new Dictionary<string, string>(comparer);
        }

        public ParameterValueDictionary(IDictionary<string, string> dictionary)
        {
            _dictionary = new Dictionary<string, string>(dictionary);
        }

        public ParameterValueDictionary(IDictionary<string, string> dictionary, IEqualityComparer<string> comparer)
        {
            _dictionary = new Dictionary<string, string>(dictionary, comparer);
        }

        public int Count
        {
            get
            {
                return _dictionary.Count;
            }
        }

        public string this[string name]
        {
            get
            {
                return Get(name);
            }
            set
            {
                Set(name, value);
            }
        }

        public bool Contains(string name)
        {
            return _dictionary.ContainsKey(name);
        }

        public string Get(string name)
        {
            string value = null;

            if (_dictionary.TryGetValue(name, out value))
            {
                return value;
            }

            return null;
        }

        public T Get<T>(string name, T defaultValue = default(T))
        {
            var strValue = Get(name);
            if (strValue == null)
            {
                return defaultValue;
            }

            var resultType = typeof(T);

            if (resultType == typeof(String))
            {
                return (T)(object)strValue;
            }

            if (resultType.IsValueType)
            {
                return (T)(object)Convert.ChangeType(strValue, resultType);
            }

            return JsonConvert.DeserializeObject<T>(strValue);
        }

        public void Set(string name, object value)
        {
            string strValue = null;

            if (value != null)
            {
                var valueType = value.GetType();

                if (valueType == typeof(String))
                {
                    strValue = (string)value;
                }
                else if (valueType.IsValueType)
                {
                    strValue = value.ToString();
                }
                else
                {
                    strValue = JsonConvert.SerializeObject(value);
                }
            }

            if (_dictionary.ContainsKey(name))
            {
                var oldValue = _dictionary[name];

                _dictionary[name] = strValue;

                if (ValueChanged != null)
                {
                    ValueChanged(this, new ParameterValueEventArgs(name, oldValue, strValue));
                }
            }
            else
            {
                _dictionary.Add(name, strValue);

                if (ValueAdded != null)
                {
                    ValueAdded(this, new ParameterValueEventArgs(name, null, strValue));
                }
            }
        }

        public bool Remove(string name)
        {
            if (!_dictionary.ContainsKey(name))
            {
                return false;
            }

            var value = _dictionary[name];
            _dictionary.Remove(name);

            if (ValueRemoved != null)
            {
                ValueRemoved(this, new ParameterValueEventArgs(name, value, null));
            }

            return true;
        }

        public Dictionary<string, string> ToDictionary()
        {
            return new Dictionary<string, string>(_dictionary, _dictionary.Comparer);
        }

        public string Serialize()
        {
            return JsonConvert.SerializeObject(_dictionary);
        }

        public static ParameterValueDictionary Deserialize(string json)
        {
            var dictionary = String.IsNullOrWhiteSpace(json) ? new Dictionary<string, string>() : JsonConvert.DeserializeObject<IDictionary<string, string>>(json);
            return new ParameterValueDictionary(dictionary);
        }

        public static ParameterValueDictionary Deserialize(string json, IEqualityComparer<string> comparer)
        {
            var dictionary = String.IsNullOrWhiteSpace(json) ? new Dictionary<string, string>() : JsonConvert.DeserializeObject<IDictionary<string, string>>(json);
            return new ParameterValueDictionary(dictionary, comparer);
        }

        public IEnumerator<KeyValuePair<string, string>> GetEnumerator()
        {
            return _dictionary.GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
