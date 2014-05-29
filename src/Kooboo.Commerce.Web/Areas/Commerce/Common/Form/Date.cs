using Kooboo.CMS.Common.Runtime.Dependency;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Kooboo.Commerce.Web.Form
{
    [Dependency(typeof(IFormControl), Key = "Date")]
    public class Date : InputControl
    {
        protected override string Type
        {
            get
            {
                return "text";
            }
        }

        public override string Name
        {
            get
            {
                return "Date";
            }
        }

        protected override void BuildControl(System.Web.Mvc.TagBuilder builder, EAV.CustomField field, string value, object htmlAttributes, System.Web.Mvc.ViewContext viewContext)
        {
            builder.MergeAttribute("data-toggle", "datepicker");
            base.BuildControl(builder, field, value, htmlAttributes, viewContext);
        }
    }
}