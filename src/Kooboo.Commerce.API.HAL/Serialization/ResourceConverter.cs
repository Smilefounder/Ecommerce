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

                // Client code will more likely use Deserialize<IListResource<T>>(), so we handle this case only
                if (!objectType.IsGenericType || (objectType.GetGenericTypeDefinition() != typeof(IListResource<>) && objectType.GetGenericTypeDefinition() != typeof(ListResource<>)))
                    throw new InvalidOperationException("Custom IListResource<T> implementation is not supported. Use IListResource<T> or ListResource<T> as the return type in deserialization.");

                var itemType = objectType.GetGenericArguments()[0];
                var resourceType = typeof(ListResource<>).MakeGenericType(itemType);

                resource = (IResource)Activator.CreateInstance(resourceType);

                var itemsArray = jObject["items"] as JArray;

                foreach (var itemJObject in itemsArray)
                {
                    var item = (IResource)itemJObject.ToObject(itemType);
                    var addMethod = resourceType.GetMethod("Add", new Type[] { itemType });
                    addMethod.Invoke(resource, new [] { item });

                    ReadHalLinks(item, itemJObject);
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
