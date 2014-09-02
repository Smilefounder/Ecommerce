using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce
{
    public static class EnumerableExtensions
    {
        public static List<TResult> ToList<T, TResult>(this IEnumerable<T> data, Func<T, TResult> transformer)
        {
            var list = new List<TResult>();

            foreach (var item in data)
            {
                list.Add(transformer(item));
            }

            return list;
        }

        public static IEnumerable<T> Except<T>(this IEnumerable<T> items, IEnumerable<T> second, Func<T, object> by)
        {
            return items.Except(second, new Comparer<T> { PropertyAccessor = by });
        }

        public static IEnumerable<T> Intersect<T>(this IEnumerable<T> items, IEnumerable<T> second, Func<T, object> by)
        {
            return items.Intersect(second, new Comparer<T> { PropertyAccessor = by });
        }

        public static IEnumerable<T> DistinctBy<T>(this IEnumerable<T> items, Func<T, object> property)
        {
            return items.Distinct(new Comparer<T> { PropertyAccessor = property });
        }

        class Comparer<T> : IEqualityComparer<T>
        {
            public Func<T, object> PropertyAccessor = null;

            public bool Equals(T x, T y)
            {
                var propX = PropertyAccessor(x);
                var propY = PropertyAccessor(y);

                return propX.Equals(propY);
            }

            public int GetHashCode(T obj)
            {
                return PropertyAccessor(obj).GetHashCode();
            }
        }

    }
}
