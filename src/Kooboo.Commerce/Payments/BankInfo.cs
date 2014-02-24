using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.Payments
{
    public class BankInfo
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public BankInfo() { }

        public BankInfo(string id, string name)
        {
            Id = id;
            Name = name;
        }
    }
}
