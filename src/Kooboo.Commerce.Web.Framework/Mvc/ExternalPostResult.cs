using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace Kooboo.Commerce.Web.Framework.Mvc
{
    public class ExternalPostResult : ActionResult
    {
        public string Url { get; set; }

        public string FormName { get; set; }

        public NameValueCollection PostParameters { get; set; }

        public ExternalPostResult(string url, string formName)
            : this(url, formName, new NameValueCollection())
        {
        }

        public ExternalPostResult(string url, string formName, NameValueCollection postParameters)
        {
            Url = url;
            FormName = formName;
            PostParameters = postParameters;
        }

        public override void ExecuteResult(ControllerContext context)
        {
            var response = context.HttpContext.Response;
            response.Clear();

            var formName = String.IsNullOrWhiteSpace(FormName) ? Guid.NewGuid().ToString("N") : FormName;

            response.Write("<html><head>");
            response.Write(string.Format("</head><body onload=\"document.{0}.submit()\">", FormName));
            response.Write(string.Format("<form name=\"{0}\" method=\"post\" action=\"{1}\" >", FormName, Url));

            if (PostParameters != null && PostParameters.Count > 0)
            {
                foreach (string key in PostParameters.AllKeys)
                {
                    response.Write(String.Format("<input name=\"{0}\" type=\"hidden\" value=\"{1}\"/>",
                        HttpUtility.HtmlEncode(key), HttpUtility.HtmlEncode(PostParameters[key])));
                }
            }

            response.Write("</form>");
            response.Write("</body></html>");
            response.End();
        }
    }
}
