using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

namespace Kooboo.Commerce.API.HAL
{
    public class PropertyResource
    {
        public PropertyResource()
        {
            IsEnumerable = false;
            ResourceNames = new List<string>();
        }

        public object Value { get; set; }
        public bool IsEnumerable { get; set; }
        public IList<string> ResourceNames { get; set; }
    }
}
