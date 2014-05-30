using Kooboo.CMS.Common.Runtime.Dependency;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace Kooboo.Commerce.Web.Form
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

        protected override string TagName
        {
            get
            {
                return "ul";
            }
        }

        protected override void BuildControl(System.Web.Mvc.TagBuilder builder, EAV.CustomField field, string value, object htmlAttributes, System.Web.Mvc.ViewContext viewContext)
        {
            builder.AddCssClass("radio-list");

            if (!String.IsNullOrWhiteSpace(field.SelectionItems))
            {
                var itemsHtml = new StringBuilder();
                var items = JsonConvert.DeserializeObject<List<SelectionListItem>>(field.SelectionItems);

                for (var i = 0; i < items.Count; i++)
                {
                    var item = items[i];
                    itemsHtml.AppendLine("<li>");

                    var radioId = field.Name + "_" + i;
                    var radio = new TagBuilder("input");
                    radio.MergeAttribute("id", radioId);
                    radio.MergeAttribute("type", "radio");
                    radio.MergeAttribute("name", field.Name);
                    radio.MergeAttribute("value", item.Value);

                    var label = new TagBuilder("label");
                    label.InnerHtml = item.Key;
                    label.AddCssClass("inline");
                    label.MergeAttribute("for", radioId);

                    itemsHtml.AppendLine(radio.ToString(TagRenderMode.SelfClosing));
                    itemsHtml.AppendLine(label.ToString());

                    itemsHtml.AppendLine("</li>");
                }

                builder.InnerHtml = itemsHtml.ToString();
            }

            base.BuildControl(builder, field, value, htmlAttributes, viewContext);
        }
    }
}