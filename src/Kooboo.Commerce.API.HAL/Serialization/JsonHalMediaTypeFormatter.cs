using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;
using System.Text;

namespace Kooboo.Commerce.API.HAL.Serialization
{
    public class JsonHalMediaTypeFormatter : JsonMediaTypeFormatter
    {
        public JsonHalMediaTypeFormatter()
        {
            SupportedMediaTypes.Clear();
            SupportedMediaTypes.Add(new MediaTypeHeaderValue("application/hal+json"));

            MediaTypeMappings.Add(new QueryStringMapping("hal", "true", new MediaTypeHeaderValue("application/hal+json")));

            SerializerSettings.Converters.Add(new ResourceConverter());

            SerializerSettings.NullValueHandling = NullValueHandling.Ignore;
            SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
            SerializerSettings.PreserveReferencesHandling = PreserveReferencesHandling.None;

#if DEBUG
            SerializerSettings.Formatting = Newtonsoft.Json.Formatting.Indented;
#endif
        }

        public override bool CanReadType(Type type)
        {
            return typeof(IResource).IsAssignableFrom(type);
        }

        public override bool CanWriteType(Type type)
        {
            return typeof(IResource).IsAssignableFrom(type);
        }
    }
}
