using Kooboo.Commerce.EAV;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.Products
{
    public class CustomFieldCollection : IEnumerable<CustomField>
    {
        private List<CustomField> _fields;
        private Action<CustomField> _onAdded;
        private Action<CustomField> _onRemoved;
        private Action _onSorted;

        public int Count
        {
            get
            {
                return _fields.Count;
            }
        }

        public CustomFieldCollection(IEnumerable<CustomField> fields, Action<CustomField> onAdded, Action<CustomField> onRemoved, Action onSorted)
        {
            _fields = fields.ToList();
            _onAdded = onAdded;
            _onRemoved = onRemoved;
            _onSorted = onSorted;
        }

        public bool Contains(string fieldName)
        {
            return _fields.Exists(f => f.Name == fieldName);
        }

        public CustomField Find(string fieldName)
        {
            return _fields.Find(f => f.Name == fieldName);
        }

        public void Intersect(IEnumerable<CustomField> fields)
        {
            foreach (var field in _fields.ToList())
            {
                if (!fields.Any(f => f.Name == field.Name))
                {
                    Remove(field);
                }
            }

            foreach (var field in fields)
            {
                if (!Contains(field.Name))
                {
                    Add(field);
                }
            }
        }

        public void Sort(IEnumerable<string> fieldNameOrders)
        {
            var fields = new List<CustomField>();

            foreach (var fieldName in fieldNameOrders)
            {
                var field = Find(fieldName);
                if (field != null)
                {
                    fields.Add(field);
                }
            }

            foreach (var field in _fields)
            {
                if (!fields.Contains(field))
                {
                    fields.Add(field);
                }
            }

            OnSorted();
        }

        private void OnSorted()
        {
            if (_onSorted != null)
            {
                _onSorted();
            }
        }

        public void Add(CustomField field)
        {
            _fields.Add(field);
            OnAdded(field);
        }

        public bool Remove(string fieldName)
        {
            var field = Find(fieldName);
            if (field != null)
            {
                return Remove(field);
            }
            return false;
        }

        public bool Remove(CustomField field)
        {
            if (_fields.Remove(field))
            {
                OnRemoved(field);
                return true;
            }
            return false;
        }

        private void OnAdded(CustomField field)
        {
            if (_onAdded != null)
            {
                _onAdded(field);
            }
        }

        private void OnRemoved(CustomField field)
        {
            if (_onRemoved != null)
            {
                _onRemoved(field);
            }
        }

        public IEnumerator<CustomField> GetEnumerator()
        {
            return _fields.GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
