using Kooboo.Commerce.Products;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.Products
{
    public class OrderedCustomFieldCollection : Collection<CustomField>
    {
        private Action<CustomField> _onAdded;
        private Action<CustomField> _onRemoved;
        private Action _onSorted;
        
        public OrderedCustomFieldCollection(IEnumerable<CustomField> fields, Action<CustomField> onAdded, Action<CustomField> onRemoved, Action onSorted)
            : base(fields.ToList())
        {
            _onAdded = onAdded;
            _onRemoved = onRemoved;
            _onSorted = onSorted;
        }

        public bool Contains(string fieldName)
        {
            return Items.Any(f => f.Name == fieldName);
        }

        public CustomField Find(int fieldId)
        {
            return Items.FirstOrDefault(f => f.Id == fieldId);
        }

        public CustomField Find(string fieldName)
        {
            return Items.FirstOrDefault(f => f.Name == fieldName);
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

            foreach (var field in Items)
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

        public void AddRange(IEnumerable<CustomField> fields)
        {
            foreach (var field in fields)
            {
                Add(field);
            }
        }

        protected override void InsertItem(int index, Kooboo.Commerce.Products.CustomField item)
        {
            base.InsertItem(index, item);
            OnAdded(item);
        }

        protected override void RemoveItem(int index)
        {
            var item = Items[index];
            base.RemoveItem(index);
            OnRemoved(item);
        }

        public bool Remove(int fieldId)
        {
            var field = Find(fieldId);
            if (field != null)
            {
                return Remove(field);
            }
            return false;
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
    }
}
