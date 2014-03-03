using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.Payments
{
    public class SupportedPaymentMethod
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public SupportedPaymentMethod() { }

        public SupportedPaymentMethod(string id, string name)
        {
            Id = id;
            Name = name;
        }

        public override string ToString()
        {
            return Name;
        }
    }
}
