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
            : base(path, data => JsonConvert.SerializeObject(data), json => JsonConvert.DeserializeObject<T>(json))
        {
        }
    }
}
