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
        [Param]
        public string CommerceInstance { get; set; }
        [Param]
        public string Language { get; set; }
        [Param]
        public string Currency { get; set; }
        [Param]
        public string ResourceName { get; set; }
        public object Data { get; set; }
    }
}
