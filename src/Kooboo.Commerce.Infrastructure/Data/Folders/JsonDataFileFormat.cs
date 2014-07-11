using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.Data.Folders
{
    public class JsonDataFileFormat : IDataFileFormat
    {
        private JsonSerializerSettings _serializerSettings;

        public JsonDataFileFormat()
            : this(null)
        {
        }

        public JsonDataFileFormat(JsonSerializerSettings serializerSettings)
        {
            _serializerSettings = serializerSettings;
        }

        public string Serialize(object content)
        {
            if (content == null)
            {
                return null;
            }

            if (_serializerSettings == null)
            {
                return JsonConvert.SerializeObject(content);
            }

            return JsonConvert.SerializeObject(content, Formatting.Indented, _serializerSettings);
        }

        public object Deserialize(string content, Type type)
        {
            if (String.IsNullOrWhiteSpace(content))
            {
                return null;
            }

            if (_serializerSettings == null)
            {
                return JsonConvert.DeserializeObject(content, type);
            }

            return JsonConvert.DeserializeObject(content, type, _serializerSettings);
        }
    }
}
