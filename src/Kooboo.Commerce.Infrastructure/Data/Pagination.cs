using Kooboo.Web.Mvc.Paging;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.Data
{
    public class Pagination : IEnumerable
    {
        private List<object> _items = new List<object>();

        protected List<object> Items
        {
            get
            {
                return _items;
            }
        }

        public int PageIndex { get; private set; }

        public int PageSize { get; private set; }

        public int TotalItems { get; private set; }

        public int TotalPages { get; private set; }

        public Pagination(IEnumerable items, int pageIndex, int pageSize, int toalItems)
        {
            foreach (var item in items)
            {
                _items.Add(item);
            }

            PageIndex = pageIndex;
            PageSize = pageSize;
            TotalItems = toalItems;
            TotalPages = (int)Math.Ceiling(TotalItems / (double)PageSize);
        }

        public IEnumerator GetEnumerator()
        {
            return _items.GetEnumerator();
        }
    }
}
