using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.API.Customers
{
    public class CustomerCustomField
    {
        public string Name { get; set; }

        public string Value { get; set; }

        public CustomerCustomField() { }

        public CustomerCustomField(string name, string value)
        {
            Name = name;
            Value = value;
        }
    }
}
