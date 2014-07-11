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

        public T Deserialize<T>(string content)
        {
            if (String.IsNullOrWhiteSpace(content))
            {
                return default(T);
            }

            return JsonConvert.DeserializeObject<T>(content);
        }
    }
}
