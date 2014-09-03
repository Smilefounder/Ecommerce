using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.Web.Framework.Json
{
    public static class JsonEncoder
    {
        public static string Encode(object data, bool usingClientConvention = false)
        {
            var settings = new JsonSerializerSettings();

            if (usingClientConvention)
            {
                settings.ContractResolver = new Newtonsoft.Json.Serialization.CamelCasePropertyNamesContractResolver();
            }

            settings.Converters.Add(new StringEnumConverter());

            return JsonConvert.SerializeObject(data, settings);
        }
    }
}
