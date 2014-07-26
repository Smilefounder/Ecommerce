using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.Api.Orders
{
    public class OrderCustomField
    {
        public int OrderId { get; set; }
        public string Name { get; set; }
        public string Value { get; set; }

        public virtual Order Order { get; set; }
    }
}
