using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.Data
{
    public class Pagination<T> : Pagination, IEnumerable<T>
    {
        public Pagination(IEnumerable<T> items, int pageIndex, int pageSize, int toalItems)
            : base(items, pageIndex, pageSize, toalItems)
        {
        }

        public Pagination<TResult> Transform<TResult>(Func<T, TResult> transform)
        {
            var items = Items.Select(i => transform((T)i));
            return new Pagination<TResult>(items, PageIndex, PageSize, TotalItems);
        }

        public new IEnumerator<T> GetEnumerator()
        {
            foreach (var item in Items)
            {
                yield return (T)item;
            }
        }
    }
}
