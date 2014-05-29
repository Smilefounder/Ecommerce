using Kooboo.Commerce.EAV;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Kooboo.Commerce.Web.Form
{
    public abstract class InputControl : FormControlBase
    {
        protected override string TagName
        {
            get
            {
                return "input";
            }
        }

        protected override TagRenderMode TagRenderMode
        {
            get
            {
                return System.Web.Mvc.TagRenderMode.SelfClosing;
            }
        }

        protected abstract string Type { get; }

        protected override void BuildControl(TagBuilder builder, CustomField field, string value, object htmlAttributes, ViewContext viewContext)
        {
            builder.MergeAttribute("type", Type);
            base.BuildControl(builder, field, value, htmlAttributes, viewContext);
        }
    }
}