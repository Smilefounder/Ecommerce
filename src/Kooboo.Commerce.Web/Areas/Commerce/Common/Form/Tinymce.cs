using Kooboo.CMS.Common.Runtime.Dependency;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Kooboo.Commerce.Web.Form
{
    [Dependency(typeof(IFormControl), Key = "Tinymce")]
    public class Tinymce : TextArea
    {
        public override string Name
        {
            get
            {
                return "Tinymce";
            }
        }

        protected override void BuildControl(System.Web.Mvc.TagBuilder builder, EAV.CustomField field, string value, object htmlAttributes, System.Web.Mvc.ViewContext viewContext)
        {
            builder.MergeAttribute("data-toggle", "tinymce");
            base.BuildControl(builder, field, value, htmlAttributes, viewContext);
        }
    }
}