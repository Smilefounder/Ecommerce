using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.Api
{
    public class Sort
    {
        public string Field { get; set; }

        public SortDirection Direction { get; set; }

        public Sort() { }

        public Sort(string field, SortDirection direction)
        {
            Field = field;
            Direction = direction;
        }
    }
}
