using Kooboo.Commerce.API;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.API.HAL.Serialization.Converters
{
    public class ResourceListConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            foreach (var @interface in objectType.GetInterfaces())
            {
                if (@interface.IsGenericType && @interface.GetGenericTypeDefinition() == typeof(IListResource<>))
                {
                    return true;
                }
            }

            return false;
        }

        public override bool CanRead
        {
            get { return false; }
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            var resource = (IEnumerable)value;

            writer.WriteStartObject();

            writer.WritePropertyName("_embedded");
            writer.WriteStartObject();

            // TODO: Descriptor provider should provide this relation "items"?
            writer.WritePropertyName("items");
            WriteItemsArray(writer, serializer, resource);

            writer.WriteEndObject();

            writer.WritePropertyName("_links");
            serializer.Serialize(writer, ((IResource)resource).Links);

            writer.WriteEndObject();
        }

        private static void WriteItemsArray(JsonWriter writer, JsonSerializer serializer, IEnumerable items)
        {
            writer.WriteStartArray();

            foreach (var item in items)
            {
                serializer.Serialize(writer, item);
            }

            writer.WriteEndArray();
        }
    }
}
