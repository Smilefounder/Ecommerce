using Kooboo.Commerce.Data.Folders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce
{
    public static class DataFolderExtensions
    {
        public static DataFile GetJsonFile(this DataFolder folder, string name)
        {
            return folder.GetFile(name, JsonDataFileFormat.Instance);
        }
    }
}
