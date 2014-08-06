using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.Data.Folders
{
    public interface IDataFileFormat
    {
        string Serialize(object content);

        object Deserialize(string content, Type type);
    }

    public static class DataFileFormats
    {
        public static readonly IDataFileFormat Json = new JsonDataFileFormat(new Newtonsoft.Json.JsonSerializerSettings
        {
            Converters =
            {
                new StringEnumConverter()
            }
        });

        public static readonly IDataFileFormat TypedJson = new JsonDataFileFormat(new Newtonsoft.Json.JsonSerializerSettings
        {
            TypeNameHandling = Newtonsoft.Json.TypeNameHandling.Auto,
            Converters =
            {
                new StringEnumConverter()
            }
        });
    }
}
