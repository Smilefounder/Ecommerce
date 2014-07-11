using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.Data.Folders
{
    public class JsonDataFileFormat : IDataFileFormat
    {
        public static readonly JsonDataFileFormat Instance = new JsonDataFileFormat();

        public string Serialize(object content)
        {
            if (content == null)
            {
                return null;
            }

            return JsonConvert.SerializeObject(content);
        }

        public object Deserialize(string content, Type type)
        {
            if (String.IsNullOrWhiteSpace(content))
            {
                return null;
            }

            return JsonConvert.DeserializeObject(content, type);
        }
    }
}
