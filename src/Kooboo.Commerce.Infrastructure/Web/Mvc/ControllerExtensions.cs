using Kooboo.CMS.Common;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;

namespace Kooboo.Commerce.Web.Mvc
{
    public static class ControllerExtensions
    {
        public static JsonResult Json(this Controller controller, object data, PropertyNaming propertyNaming, JsonRequestBehavior behavior = JsonRequestBehavior.AllowGet)
        {
            var jsonResult = new JsonNetResult();
            if (propertyNaming == PropertyNaming.CamelCase)
            {
                jsonResult.Settings.ContractResolver = new CamelCasePropertyNamesContractResolver();
            }

            jsonResult.Data = data;
            jsonResult.JsonRequestBehavior = behavior;
            return jsonResult;
        }

        public static JsonResultData CurrentAjaxFormResult(this Controller controller)
        {
            return controller.TempData["CurrentAjaxFormResult"] as JsonResultData;
        }
    }
}
