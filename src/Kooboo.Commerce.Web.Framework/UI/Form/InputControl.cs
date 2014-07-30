using Kooboo.Commerce.Products;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Kooboo.Commerce.Web.Framework.UI.Form
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

        protected override void BuildControl(TagBuilder builder, CustomFieldDefinition field, string value, object htmlAttributes, ViewContext viewContext)
        {
            builder.MergeAttribute("type", Type, true);

            if (Type == "text")
            {
                builder.MergeAttribute("value", value, true);
            }

            base.BuildControl(builder, field, value, htmlAttributes, viewContext);
        }
    }
}