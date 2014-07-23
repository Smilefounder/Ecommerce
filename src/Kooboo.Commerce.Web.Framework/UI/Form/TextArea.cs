using Kooboo.CMS.Common.Runtime.Dependency;
using Kooboo.Commerce.EAV;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Kooboo.Commerce.Web.Framework.UI.Form
{
    [Dependency(typeof(IFormControl), Key = "TextArea")]
    public class TextArea : FormControlBase
    {
        public override string Name
        {
            get
            {
                return "TextArea";
            }
        }

        protected override string TagName
        {
            get
            {
                return "textarea";
            }
        }

        protected override System.Web.Mvc.TagRenderMode TagRenderMode
        {
            get
            {
                return System.Web.Mvc.TagRenderMode.Normal;
            }
        }

        public override string ValueBindingName
        {
            get
            {
                return "value";
            }
        }
    }
}