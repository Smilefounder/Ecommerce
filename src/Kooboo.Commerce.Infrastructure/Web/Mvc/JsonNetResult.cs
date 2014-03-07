using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Kooboo.Commerce.Web.Mvc
{
    public class JsonNetResult : JsonResult
    {

        public JsonNetResult()
            : base()
        {
            Settings = new JsonSerializerSettings();
            Settings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
            Settings.PreserveReferencesHandling = PreserveReferencesHandling.None;
            Settings.ContractResolver = new EFContractResolver();
            JsonRequestBehavior = System.Web.Mvc.JsonRequestBehavior.AllowGet;
        }


        public override void ExecuteResult(ControllerContext context)
        {
            if (JsonRequestBehavior == JsonRequestBehavior.DenyGet &&
                           String.Equals(context.HttpContext.Request.HttpMethod, "GET", StringComparison.OrdinalIgnoreCase))
            {
                throw new InvalidOperationException("Json GET request is not allowed.");
            } 
            
            HttpResponseBase response = context.HttpContext.Response;
            response.ContentType = !String.IsNullOrEmpty(ContentType) ? ContentType : "application/json";

            if (ContentEncoding != null)
            {
                response.ContentEncoding = ContentEncoding;
            }

            if (Data != null)
            {
#if DEBUG
                string json = JsonConvert.SerializeObject(Data, Formatting.Indented, Settings);
#else
                string json = JsonConvert.SerializeObject(Data, Formatting.None, Settings);
#endif
                response.Write(json);
            }
        }

        public JsonSerializerSettings Settings { get; private set; }

        public JsonNetResult Camelcased()
        {
            Settings.ContractResolver = new CamelCasePropertyNamesContractResolver();
            return this;
        }
    }
}
