﻿using Kooboo.Commerce.Products;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Kooboo.Commerce.Web.Framework.UI.Form.Validation;

namespace Kooboo.Commerce.Web.Framework.UI.Form
{
    public abstract class FormControlBase : IFormControl
    {
        public abstract string Name { get; }

        public virtual string ValueBindingName
        {
            get
            {
                return "value";
            }
        }

        public abstract bool IsValuesPredefined { get; }

        public abstract bool IsSelectionList { get; }

        protected abstract string TagName { get; }

        protected virtual TagRenderMode TagRenderMode
        {
            get
            {
                return System.Web.Mvc.TagRenderMode.Normal;
            }
        }

        public virtual IHtmlString Render(CustomFieldDefinition fieldDefinition, string fieldValue, object htmlAttributes, ViewContext viewContext)
        {
            var builder = new TagBuilder(TagName);

            builder.MergeAttribute("name", fieldDefinition.Name);

            var validationAttributes = fieldDefinition.GetUnobtrusiveValidationAtributes();
            if (validationAttributes != null && validationAttributes.Count > 0)
            {
                builder.MergeAttributes(validationAttributes, true);
            }

            BuildControl(builder, fieldDefinition, fieldValue, htmlAttributes, viewContext);

            return new HtmlString(builder.ToString(TagRenderMode));
        }

        protected virtual void BuildControl(TagBuilder builder, CustomFieldDefinition fieldDefinition, string fieldValue, object htmlAttributes, ViewContext viewContext)
        {
            if (htmlAttributes != null)
            {
                builder.MergeAttributes(HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes), true);
            }
        }

        public virtual string GetFieldDisplayText(CustomFieldDefinition fieldDefinition, string fieldValue)
        {
            return fieldValue;
        }
    }
}