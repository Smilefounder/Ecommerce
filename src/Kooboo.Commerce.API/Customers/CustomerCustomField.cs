using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.API.Customers
{
    public class CustomerCustomField
    {
        public int CustomerId { get; set; }
        public string Name { get; set; }
        public string Value { get; set; }

        public virtual Customer Customer { get; set; }
    }
}
