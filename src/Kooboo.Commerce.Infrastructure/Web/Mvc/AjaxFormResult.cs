using Kooboo.CMS.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;

namespace Kooboo.Commerce.Web.Mvc
{
    public class AjaxFormResult : ActionResult
    {
        public JsonResultData Data { get; private set; }

        public AjaxFormResult(ModelStateDictionary modelState)
        {
            Data = new JsonResultData(modelState);
        }

        public AjaxFormResult RedirectTo(string url)
        {
            Data.RedirectUrl = url;
            return this;
        }

        public AjaxFormResult ReloadPage()
        {
            Data.ReloadPage = true;
            return this;
        }

        public override void ExecuteResult(ControllerContext context)
        {
            var result = new JsonResult
            {
                Data = Data,
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };

            result.ExecuteResult(context);
        }
    }
}
