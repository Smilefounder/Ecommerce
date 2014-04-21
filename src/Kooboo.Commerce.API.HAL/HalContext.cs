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
        public string CommerceInstance { get; set; }
        public string Language { get; set; }
        public string Currency { get; set; }
        public string ResourceName { get; set; }
        public object Data { get; set; }
    }
}
