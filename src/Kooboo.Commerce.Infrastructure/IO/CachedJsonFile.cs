using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.IO
{
    class CachedJsonFile<T> : CachedFile<T>
        where T : class
    {
        public CachedJsonFile(string path)
            : base(path, Serialize, Deserialize)
        {
        }

        static string Serialize(T data)
        {
            return JsonConvert.SerializeObject(data, Formatting.Indented);
        }

        static T Deserialize(string json)
        {
            return JsonConvert.DeserializeObject<T>(json);
        }
    }
}
