using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.Api
{
    public class QuerySort
    {
        public string Field { get; set; }

        public SortDirection Direction { get; set; }

        public QuerySort() { }

        public QuerySort(string field, SortDirection direction)
        {
            Field = field;
            Direction = direction;
        }
    }
}
