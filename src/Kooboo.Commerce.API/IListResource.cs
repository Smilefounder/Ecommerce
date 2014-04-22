using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.API
{
    public interface IListResource<T> : IResource, IEnumerable<T>
        where T : IItemResource
    {
    }

    public class ListResource<T> : IListResource<T>
        where T : IItemResource
    {
        private List<T> _items;

        public int Count
        {
            get
            {
                return _items.Count;
            }
        }

        public IList<Link> Links { get; set; }

        public ListResource()
        {
            _items = new List<T>();
        }

        public ListResource(IEnumerable<T> items)
        {
            _items = items.ToList();
        }

        public void Add(T item)
        {
            _items.Add(item);
        }

        public void AddRange(IEnumerable<T> items)
        {
            _items.AddRange(items);
        }

        public bool Remove(T item)
        {
            return _items.Remove(item);
        }

        public void Clear()
        {
            _items.Clear();
        }

        public IEnumerator<T> GetEnumerator()
        {
            return _items.GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
