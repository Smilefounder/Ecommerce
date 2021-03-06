﻿using Kooboo.CMS.Common.Runtime.Dependency;
using Kooboo.Commerce.Products;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace Kooboo.Commerce.Web.Framework.UI.Form
{
    [Dependency(typeof(IFormControl), Key = "CheckBoxList")]
    public class CheckBoxList : FormControlBase
    {
        public override string Name
        {
            get { return "CheckBoxList"; }
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
            get { return "ul"; }
        }

        public override string ValueBindingName
        {
            get { return "checkboxlist"; }
        }

        protected override void BuildControl(System.Web.Mvc.TagBuilder builder, CustomFieldDefinition field, string value, object htmlAttributes, System.Web.Mvc.ViewContext viewContext)
        {
            builder.AddCssClass("form-list");

            var itemsHtml = new StringBuilder();
            var i = 0;

            foreach (var item in field.SelectionItems)
            {
                itemsHtml.AppendLine("<li>");

                var checkboxId = field.Name + "_" + i;
                var checkbox = new TagBuilder("input");
                checkbox.MergeAttribute("id", checkboxId);
                checkbox.MergeAttribute("type", "checkbox");
                checkbox.MergeAttribute("name", field.Name);
                checkbox.MergeAttribute("value", item.Value);

                var label = new TagBuilder("label");
                label.InnerHtml = item.Text;
                label.AddCssClass("inline");
                label.MergeAttribute("for", checkboxId);

                itemsHtml.AppendLine(checkbox.ToString(TagRenderMode.SelfClosing));
                itemsHtml.AppendLine(label.ToString());

                itemsHtml.AppendLine("</li>");

                i++;
            }

            builder.InnerHtml = itemsHtml.ToString();

            base.BuildControl(builder, field, value, htmlAttributes, viewContext);
        }

        public override string GetFieldDisplayText(CustomFieldDefinition fieldDefinition, string fieldValue)
        {
            var item = fieldDefinition.SelectionItems.FirstOrDefault(i => i.Value == fieldValue);
            return item == null ? fieldValue : item.Text;
        }
    }
}