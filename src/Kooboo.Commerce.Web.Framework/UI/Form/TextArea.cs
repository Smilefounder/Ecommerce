using Kooboo.CMS.Common.Runtime.Dependency;
using Kooboo.Commerce.Products;
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

        public override bool IsValuesPredefined
        {
            get { return false; }
        }

        public override bool IsSelectionList
        {
            get { return false; }
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

        protected override void BuildControl(System.Web.Mvc.TagBuilder builder, CustomField field, string value, object htmlAttributes, System.Web.Mvc.ViewContext viewContext)
        {
            base.BuildControl(builder, field, value, htmlAttributes, viewContext);
            builder.InnerHtml = value;
        }
    }
}