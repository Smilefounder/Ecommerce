using Kooboo.CMS.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;

namespace Kooboo.Commerce.Web.Framework.Mvc
{
    public class AjaxFormResult : ActionResult
    {
        public JsonResultData Data { get; private set; }

        public AjaxFormResult()
        {
            Data = new JsonResultData();
        }

        public AjaxFormResult(ModelStateDictionary modelState)
        {
            Data = new JsonResultData(modelState);
        }

        public AjaxFormResult Success()
        {
            Data.Success = true;
            return this;
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

        public AjaxFormResult WithModel(object model)
        {
            Data.Model = model;
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
