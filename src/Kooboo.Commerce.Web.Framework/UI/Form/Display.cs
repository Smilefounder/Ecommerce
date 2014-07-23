using Kooboo.CMS.Common.Runtime.Dependency;
using Kooboo.Commerce.EAV;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace Kooboo.Commerce.Web.Framework.UI.Form
{
    [Dependency(typeof(IFormControl), Key = "Display")]
    public class Display : IFormControl
    {
        public string Name
        {
            get
            {
                return "Display";
            }
        }

        public string ValueBindingName
        {
            get
            {
                return "value";
            }
        }

        public IHtmlString Render(CustomField field, string value, object htmlAttributes, System.Web.Mvc.ViewContext viewContext)
        {
            var input = new TagBuilder("input");
            input.Attributes.Add("type", "hidden");
            input.Attributes.Add("name", field.Name);

            var text = new TagBuilder("span");
            text.InnerHtml = field.DefaultValue;

            var html = new StringBuilder();
            html.AppendLine(input.ToString(TagRenderMode.SelfClosing));
            html.AppendLine(text.ToString());

            return new HtmlString(html.ToString());
        }
    }
}