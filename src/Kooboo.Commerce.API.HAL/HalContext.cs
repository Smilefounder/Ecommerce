using Kooboo.Commerce.Rules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace Kooboo.Commerce.API.HAL
{
    public class HalContext
    {
        public HttpContext WebContext { get; set; }
        [Parameter(Name = "CommerceInstance", DisplayName = "Commerce Instance")]
        public string CommerceInstance { get; set; }
        [Parameter(Name = "Language", DisplayName = "Language")]
        public string Language { get; set; }
        [Parameter(Name = "Currency", DisplayName = "Currency")]
        public string Currency { get; set; }
        [Parameter(Name = "ResourceName", DisplayName = "Resource Name")]
        public string ResourceName { get; set; }
        public object Data { get; set; }
    }
}
