using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.Web.Framework.Json
{
    public static class JsonEncoder
    {
        public static string Encode(object data, bool camelCase = false)
        {
            var settings = new JsonSerializerSettings();

            if (camelCase)
            {
                settings.ContractResolver = new Newtonsoft.Json.Serialization.CamelCasePropertyNamesContractResolver();
            }

            return JsonConvert.SerializeObject(data, settings);
        }
    }
}
