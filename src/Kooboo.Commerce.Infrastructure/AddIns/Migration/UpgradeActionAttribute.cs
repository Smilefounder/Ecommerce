using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.AddIns.Migration
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
    public class UpgradeActionAttribute : Attribute
    {
        public string Version { get; set; }

        public string Description { get; set; }

        public int Order { get; set; }

        public UpgradeActionAttribute(string version, string description, int order)
        {
            Version = version;
        }
    }
}
