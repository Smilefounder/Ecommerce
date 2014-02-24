using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using Kooboo.Web.Mvc;
using Kooboo.CMS.Common.Runtime.Dependency;
using Kooboo.Commerce.EAV.WebControls;

namespace Kooboo.Commerce.EAV.WebControls
{
    [Dependency(typeof(IWebControl), Key = "Kooboo.Commerce.EAV.WebControls.TextBox")]
    public class TextBox : WebControlBase
    {
        public override string Name
        {
            get
            {
                return "TextBox";
            }
        }
    }
}