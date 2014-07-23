using Kooboo.CMS.Common.Runtime.Dependency;
using Kooboo.Commerce.EAV;
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

        protected override string TagName
        {
            get
            {
                return "select";
            }
        }

        protected override void BuildControl(System.Web.Mvc.TagBuilder builder, CustomField field, string value, object htmlAttributes, System.Web.Mvc.ViewContext viewContext)
        {
            base.BuildControl(builder, field, value, htmlAttributes, viewContext);

            if (!String.IsNullOrWhiteSpace(field.SelectionItems))
            {
                var html = new StringBuilder();

                html.Append("<option value=\"\"></option>");

                var items = JsonConvert.DeserializeObject<IList<SelectionListItem>>(field.SelectionItems);
                foreach (var item in items)
                {
                    html.AppendFormat("<option value=\"{0}\">{1}</option>", item.Value, item.Key)
                        .AppendLine();
                }

                builder.InnerHtml = html.ToString();
            }
        }
    }
}