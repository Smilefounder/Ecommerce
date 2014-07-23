using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.Globalization
{
    public class TextDictionary : IDictionary<string, string>
    {
        private IDictionary<string, string> _data;

        public TextDictionary()
            : this(null)
        {
        }

        public TextDictionary(IDictionary<string, string> data)
        {
            _data = data ?? new Dictionary<string, string>();
        }

        public void Add(string key, string value)
        {
            _data.Add(key, value);
        }

        public bool ContainsKey(string key)
        {
            return _data.ContainsKey(key);
        }

        public ICollection<string> Keys
        {
            get
            {
                return _data.Keys;
            }
        }

        public bool Remove(string key)
        {
            return _data.Remove(key);
        }

        public bool TryGetValue(string key, out string value)
        {
            return _data.TryGetValue(key, out value);
        }

        public ICollection<string> Values
        {
            get { return _data.Values; }
        }

        public string this[string key]
        {
            get
            {
                string value;
                if (_data.TryGetValue(key, out value))
                {
                    return value;
                }

                return null;
            }
            set
            {
                if (value == null)
                {
                    _data.Remove(key);
                    return;
                }

                if (_data.ContainsKey(key))
                {
                    _data[key] = value;
                }
                else
                {
                    _data.Add(key, value);
                }
            }
        }

        public void Add(KeyValuePair<string, string> item)
        {
            _data.Add(item);
        }

        public void Clear()
        {
            _data.Clear();
        }

        public bool Contains(KeyValuePair<string, string> item)
        {
            return _data.Contains(item);
        }

        public void CopyTo(KeyValuePair<string, string>[] array, int arrayIndex)
        {
            _data.CopyTo(array, arrayIndex);
        }

        public int Count
        {
            get
            {
                return _data.Count;
            }
        }

        bool ICollection<KeyValuePair<string, string>>.IsReadOnly
        {
            get { return _data.IsReadOnly; }
        }

        public bool Remove(KeyValuePair<string, string> item)
        {
            return _data.Remove(item);
        }

        public IEnumerator<KeyValuePair<string, string>> GetEnumerator()
        {
            return _data.GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
