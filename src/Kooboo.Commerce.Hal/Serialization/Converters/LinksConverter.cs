using Kooboo.Commerce.API;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.HAL.Serialization.Converters
{
    public class LinksConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return typeof(IEnumerable<Link>).IsAssignableFrom(objectType);
        }

        public override bool CanRead
        {
            get
            {
                return false;
            }
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            var links = (IEnumerable<Link>)value;

            writer.WriteStartObject();

            foreach (var link in links)
            {
                WriteLink(writer, serializer, link);
            }

            writer.WriteEndObject();
        }

        private static void WriteLink(JsonWriter writer, JsonSerializer serializer, Link link)
        {
            writer.WritePropertyName(link.Rel);

            writer.WriteStartObject();

            writer.WritePropertyName("href");
            serializer.Serialize(writer, link.Href);

            if (link.IsTemplated)
            {
                writer.WritePropertyName("templated");
                serializer.Serialize(writer, link.IsTemplated);
            }

            writer.WriteEndObject();
        }
    }
}
