using Kooboo.CMS.Common.Runtime.Dependency;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace Kooboo.Commerce.Web.Framework.UI.Form
{
    [Dependency(typeof(IFormControl), Key = "RadioList")]
    public class RadioList : FormControlBase
    {
        public override string Name
        {
            get
            {
                return "RadioList";
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
                return "ul";
            }
        }

        public override string ValueBindingName
        {
            get
            {
                return "radiolist";
            }
        }

        protected override void BuildControl(System.Web.Mvc.TagBuilder builder, Products.CustomFieldDefinition field, string value, object htmlAttributes, System.Web.Mvc.ViewContext viewContext)
        {
            builder.AddCssClass("form-list");

            var itemsHtml = new StringBuilder();
            var i = 0;

            foreach (var item in field.SelectionItems)
            {
                itemsHtml.AppendLine("<li>");

                var radioId = field.Name + "_" + i;
                var radio = new TagBuilder("input");
                radio.MergeAttribute("id", radioId);
                radio.MergeAttribute("type", "radio");
                radio.MergeAttribute("name", field.Name);
                radio.MergeAttribute("value", item.Value);

                var label = new TagBuilder("label");
                label.InnerHtml = item.Text;
                label.AddCssClass("inline");
                label.MergeAttribute("for", radioId);

                itemsHtml.AppendLine(radio.ToString(TagRenderMode.SelfClosing));
                itemsHtml.AppendLine(label.ToString());

                itemsHtml.AppendLine("</li>");

                i++;
            }

            builder.InnerHtml = itemsHtml.ToString();

            base.BuildControl(builder, field, value, htmlAttributes, viewContext);
        }
    }
}