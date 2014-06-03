using Kooboo.CMS.Common.Runtime.Dependency;
using Kooboo.Commerce.EAV;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Kooboo.Commerce.Web.Form.Validation;

namespace Kooboo.Commerce.Web.Form
{
    [Dependency(typeof(IFormControl), Key = "File")]
    public class File : IFormControl
    {
        public string Name
        {
            get
            {
                return "File";
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
            input.Attributes.Add("type", "file");
            input.Attributes.Add("id", field.Name);
            input.Attributes.Add("name", field.Name);

            if (htmlAttributes != null)
            {
                input.MergeAttributes(HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes));
            }

            input.MergeAttributes(field.GetUnobtrusiveValidationAtributes());

            return new HtmlString(input.ToString(TagRenderMode.SelfClosing));
        }
    }
}