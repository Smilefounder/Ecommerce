using Kooboo.CMS.Common.Runtime.Dependency;
using Kooboo.Commerce.EAV;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Kooboo.Commerce.Web.Framework.UI.Form
{
    [Dependency(typeof(IFormControl), Key = "TextBox")]
    public class TextBox : InputControl
    {
        public override string Name
        {
            get
            {
                return "TextBox";
            }
        }

        protected override string Type
        {
            get
            {
                return "text";
            }
        }
    }
}