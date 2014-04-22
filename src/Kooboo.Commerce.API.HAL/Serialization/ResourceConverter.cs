using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.API.HAL.Serialization
{
    public class ResourceConverter : JsonConverter
    {
        public Func<bool> GenerateHalLinks { get; set; }

        public ResourceConverter()
        {
            GenerateHalLinks = () => true;
        }

        public override bool CanConvert(Type objectType)
        {
            return typeof(IResource).IsAssignableFrom(objectType);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            var jObject = JObject.Load(reader);
            IResource resource = null;

            // Read data
            if (typeof(IEnumerable).IsAssignableFrom(objectType))
            {
                var itemsArray = jObject["items"] as JArray;
                resource = (IResource)itemsArray.ToObject(objectType);

                var index = 0;

                foreach (var item in (IEnumerable)resource)
                {
                    var itemJObject = itemsArray[index];
                    ReadHalLinks((IResource)item, itemJObject);
                    index++;
                }
            }
            else
            {
                resource = (IResource)jObject.ToObject(objectType);
            }

            ReadHalLinks(resource, jObject);

            return resource;
        }

        private void ReadHalLinks(IResource resource, JToken token)
        {
            var linksToken = token["_links"];
            if (linksToken != null)
            {
                foreach (var prop in linksToken.Children<JProperty>())
                {
                    var relation = prop.Name;
                    var link = prop.Value.ToObject<Link>();
                    link.Rel = relation;
                    resource.Links.Add(link);
                }
            }
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            var resource = (IResource)value;
            writer.WriteStartObject();

            if (resource is IEnumerable)
            {
                writer.WritePropertyName("items");
                writer.WriteStartArray();

                foreach (var item in (IEnumerable)resource)
                {
                    serializer.Serialize(writer, item);
                }

                writer.WriteEndArray();
            }
            else
            {
                foreach (var token in JToken.FromObject(resource))
                {
                    serializer.Serialize(writer, token);
                }
            }

            WriteHalLinks(writer, serializer, resource.Links);

            writer.WriteEndObject();
        }

        static void WriteHalLinks(JsonWriter writer, JsonSerializer serializer, IEnumerable<Link> links)
        {
            writer.WritePropertyName("_links");
            writer.WriteStartObject();

            foreach (var link in links)
            {
                WriteHalLink(writer, serializer, link);
            }

            writer.WriteEndObject();
        }

        static void WriteHalLink(JsonWriter writer, JsonSerializer serializer, Link link)
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
