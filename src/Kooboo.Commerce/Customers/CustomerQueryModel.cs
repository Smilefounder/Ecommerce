using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.Customers
{
    public class CustomerQueryModel
    {
        public Customer Customer { get; set; }
        public int OrdersCount { get; set; }
    }
}
