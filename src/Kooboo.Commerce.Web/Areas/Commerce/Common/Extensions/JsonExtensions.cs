using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Kooboo.Commerce.Web
{
    public static class JsonExtensions
    {
        public static string ToJson(this object data, PropertyNaming propertyNaming = PropertyNaming.Default)
        {
            if (data == null)
            {
                return null;
            }

            var settings = new JsonSerializerSettings();

            if (propertyNaming == PropertyNaming.CamelCase)
            {
                settings.ContractResolver = new CamelCasePropertyNamesContractResolver();
            }

            settings.Converters.Add(new StringEnumConverter());

            return JsonConvert.SerializeObject(data, settings);
        }
    }
}