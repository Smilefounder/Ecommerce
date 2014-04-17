using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Kooboo.Commerce.HAL.Serialization
{
    public class Resource
    {
        public string Name { get; set; }

        public object Data { get; set; }

        public IList<Link> Links { get; set; }

        public Resource()
        {
            Links = new List<Link>();
        }
    }
}
