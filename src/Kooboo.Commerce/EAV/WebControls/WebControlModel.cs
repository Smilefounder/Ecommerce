using Kooboo.Commerce.EAV;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;

namespace Kooboo.Commerce.EAV.WebControls
{
    public class WebControlModel
    {
        public string ClientId { get; set; }

        public string UniqueName { get; set; }

        public string Value { get; set; }

        public CustomField CustomField { get; set; }
    }
}
