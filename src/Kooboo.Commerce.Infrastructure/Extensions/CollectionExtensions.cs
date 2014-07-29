using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce
{
    public static class CollectionExtensions
    {
        public static void Update<T>(this ICollection<T> collection, IEnumerable<T> from, Func<T, object> by, Action<T, T> onUpdateItem, Action<T> onAddItem = null, Action<T> onRemoveItem = null)
        {
            var itemsToAdd = new List<T>();

            foreach (var item in from)
            {
                if (!collection.Any(it => by(it).Equals(by(item))))
                {
                    itemsToAdd.Add(item);
                }
            }

            var itemsToRemove = new List<T>();

            foreach (var oldItem in collection.ToList())
            {
                var update = from.FirstOrDefault(it => by(it).Equals(by(oldItem)));
                if (update != null)
                {
                    onUpdateItem(oldItem, update);
                }
                else
                {
                    itemsToRemove.Add(oldItem);
                }
            }

            foreach (var item in itemsToRemove)
            {
                collection.Remove(item);
                if (onRemoveItem != null)
                {
                    onRemoveItem(item);
                }
            }
            foreach (var item in itemsToAdd)
            {
                collection.Add(item);
                if (onAddItem != null)
                {
                    onAddItem(item);
                }
            }
        }
    }
}
