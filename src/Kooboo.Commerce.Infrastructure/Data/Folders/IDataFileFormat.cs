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
        public static readonly IDataFileFormat Json = new JsonDataFileFormat();
    }
}
