using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.Web.Framework.Json
{
    public class PreserveDictionaryKeyCaseConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return typeof(IDictionary).IsAssignableFrom(objectType);
        }

        public override bool CanRead
        {
            get { return false; }
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            throw new InvalidOperationException();
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            IDictionary dictionary = (IDictionary)value;

            writer.WriteStartObject();

            foreach (DictionaryEntry entry in dictionary)
            {
                string key = Convert.ToString(entry.Key, CultureInfo.InvariantCulture);
                writer.WritePropertyName(key);
                serializer.Serialize(writer, entry.Value);
            }

            writer.WriteEndObject();
        }
    }
}
