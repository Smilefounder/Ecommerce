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
        public object Data { get; set; }

        public ResourceDescriptor RequestResource { get; set; }
    }
}
