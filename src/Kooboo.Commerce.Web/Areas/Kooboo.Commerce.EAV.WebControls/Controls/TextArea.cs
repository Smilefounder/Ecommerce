using Kooboo.CMS.Common.Runtime.Dependency;
using Kooboo.Commerce.EAV.WebControls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Kooboo.Commerce.EAV.WebControls
{
    [Dependency(typeof(IWebControl), Key = "Kooboo.Commerce.EAV.WebControls.TextArea")]
    public class TextArea : WebControlBase
    {
        public override string Name
        {
            get
            {
                return "TextArea";
            }
        }
    }
}