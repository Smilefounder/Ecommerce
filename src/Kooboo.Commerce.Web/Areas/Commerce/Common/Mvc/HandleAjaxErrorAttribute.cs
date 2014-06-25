using Kooboo.Commerce.Web.Framework.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Kooboo.Commerce.Web.Mvc
{
    public class HandleAjaxErrorAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            if (filterContext.Exception != null && filterContext.HttpContext.Request.IsAjaxRequest())
            {
                filterContext.Result = new JsonNetResult
                {
                    Data = new
                    {
                        Success = false,
                        Message = filterContext.Exception.Message
                    }
                }
                .UsingClientConvention();

                filterContext.ExceptionHandled = true;
                filterContext.HttpContext.Response.StatusCode = 500;
            }

            base.OnActionExecuted(filterContext);
        }
    }
}