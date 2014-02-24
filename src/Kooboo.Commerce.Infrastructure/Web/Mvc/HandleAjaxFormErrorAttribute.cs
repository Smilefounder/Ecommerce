using Kooboo.CMS.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;

namespace Kooboo.Commerce.Web.Mvc
{
    public class HandleAjaxFormErrorAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            if (filterContext.Exception != null)
            {
                var result = filterContext.Result as AjaxFormResult;
                if (result == null)
                {
                    result = new AjaxFormResult(((Controller)filterContext.Controller).ModelState);
                    filterContext.Result = result;
                }

                result.Data.AddException(filterContext.Exception);
                filterContext.ExceptionHandled = true;
            }

            base.OnActionExecuted(filterContext);
        }
    }
}
