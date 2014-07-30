using Kooboo.CMS.Common.Runtime.Dependency;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Kooboo.Commerce.Web.Framework.UI.Form
{
    [Dependency(typeof(IFormControl), Key = "RichTextEditor")]
    public class RichTextEditor : TextArea
    {
        public override string Name
        {
            get
            {
                return "RichTextEditor";
            }
        }

        protected override void BuildControl(System.Web.Mvc.TagBuilder builder, Products.CustomFieldDefinition field, string value, object htmlAttributes, System.Web.Mvc.ViewContext viewContext)
        {
            builder.MergeAttribute("data-toggle", "tinymce");
            base.BuildControl(builder, field, value, htmlAttributes, viewContext);
        }
    }
}