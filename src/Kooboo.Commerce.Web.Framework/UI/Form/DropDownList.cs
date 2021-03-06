﻿using Kooboo.CMS.Common.Runtime.Dependency;
using Kooboo.Commerce.Products;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace Kooboo.Commerce.Web.Framework.UI.Form
{
    [Dependency(typeof(IFormControl), Key = "DropDownList")]
    public class DropDownList : FormControlBase
    {
        public override string Name
        {
            get
            {
                return "DropDownList";
            }
        }

        public override bool IsValuesPredefined
        {
            get { return true; }
        }

        public override bool IsSelectionList
        {
            get { return true; }
        }

        protected override string TagName
        {
            get
            {
                return "select";
            }
        }

        protected override void BuildControl(System.Web.Mvc.TagBuilder builder, CustomFieldDefinition field, string value, object htmlAttributes, System.Web.Mvc.ViewContext viewContext)
        {
            base.BuildControl(builder, field, value, htmlAttributes, viewContext);

            var html = new StringBuilder();
            html.Append("<option value=\"\"></option>");

            foreach (var item in field.SelectionItems)
            {
                html.AppendFormat("<option value=\"{0}\">{1}</option>", item.Value, item.Text)
                    .AppendLine();
            }

            builder.InnerHtml = html.ToString();
        }

        public override string GetFieldDisplayText(CustomFieldDefinition fieldDefinition, string fieldValue)
        {
            var item = fieldDefinition.SelectionItems.FirstOrDefault(i => i.Value == fieldValue);
            return item == null ? fieldValue : item.Text;
        }
    }
}