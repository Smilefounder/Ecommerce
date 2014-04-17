using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.HAL.Serialization.Converters
{
    public class ResourceConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return typeof(Resource).IsAssignableFrom(objectType);
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
            var resource = (Resource)value;

            writer.WriteStartObject();

            if (resource.Data is IEnumerable)
            {
                writer.WritePropertyName("_embedded");
                writer.WriteStartObject();

                // TODO: Descriptor provider should provide this relation "items"?
                writer.WritePropertyName("items");
                serializer.Serialize(writer, resource.Data);

                writer.WriteEndObject();
            }
            else
            {
                var token = JToken.FromObject(resource.Data);

                foreach (var prop in token)
                {
                    prop.WriteTo(writer);
                }
            }

            serializer.Serialize(writer, resource.Links);

            writer.WriteEndObject();
        }
    }
}
