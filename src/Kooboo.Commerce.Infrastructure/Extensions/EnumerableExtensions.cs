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
    }
}
