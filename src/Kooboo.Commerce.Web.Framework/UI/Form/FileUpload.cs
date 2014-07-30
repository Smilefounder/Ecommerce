using Kooboo.CMS.Common.Runtime.Dependency;
using Kooboo.Commerce.Products;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace Kooboo.Commerce.Web.Framework.UI.Form
{
    [Dependency(typeof(IFormControl), Key = "File")]
    public class FileUpload : IFormControl
    {
        public string Name
        {
            get
            {
                return "File";
            }
        }

        public bool IsValuesPredefined
        {
            get { return false; }
        }

        public bool IsSelectionList
        {
            get { return false; }
        }

        public string ValueBindingName
        {
            get
            {
                return "value";
            }
        }

        public IHtmlString Render(CustomFieldDefinition field, string value, object htmlAttributes, System.Web.Mvc.ViewContext viewContext)
        {
            var container = new TagBuilder("div");
            container.AddCssClass("custom-file");
            container.Attributes.Add("data-toggle", "fileupload");

            var textbox = new TagBuilder("input");
            textbox.Attributes.Add("type", "text");
            textbox.Attributes.Add("name", field.Name);
            textbox.MergeAttributes(field.GetUnobtrusiveValidationAtributes());

            RouteValueDictionary additionHtmlAttributes = null;

            if (htmlAttributes != null)
            {
                additionHtmlAttributes = HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes);

                var databind = additionHtmlAttributes["data-bind"];
                if (databind != null)
                {
                    additionHtmlAttributes.Remove("data-bind");
                    textbox.MergeAttribute("data-bind", databind.ToString());
                }

                container.MergeAttributes(additionHtmlAttributes);
            }

            var button = new TagBuilder("a");
            button.AddCssClass("button");

            var span = new TagBuilder("span");

            var fileInput = new TagBuilder("input");
            fileInput.Attributes.Add("type", "file");
            fileInput.Attributes.Add("id", field.Name);
            fileInput.Attributes.Add("name", field.Name);

            span.InnerHtml = fileInput.ToString(TagRenderMode.SelfClosing);
            button.InnerHtml = "Browse..." + span.ToString();

            container.InnerHtml = String.Concat(textbox.ToString(TagRenderMode.SelfClosing), button.ToString());

            return new HtmlString(container.ToString());
        }
    }
}