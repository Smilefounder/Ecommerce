using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.API.HAL
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
    public class ResourceAttribute : Attribute
    {
        public ResourceAttribute()
        {
        }

        public ResourceAttribute(string name, string uri = null, bool isList = false, string itemName = null)
        {
            Name = name;
            Uri = uri;
            IsList = isList;
            ItemName = itemName;
        }

        public string Name { get; set; }
        public string Uri { get; set; }
        public bool IsList { get; set; }

        private string itemName;
        public string ItemName
        {
            get { return itemName; }
            set
            {
                if (!string.IsNullOrEmpty(value))
                {
                    IsList = true;
                }
                itemName = value;
            }
        }
    }
}
