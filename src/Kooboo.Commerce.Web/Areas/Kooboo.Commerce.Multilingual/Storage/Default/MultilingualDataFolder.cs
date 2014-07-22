using Kooboo.Commerce.Data.Folders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Kooboo.Commerce.Multilingual.Storage.Default
{
    public static class MultilingualDataFolder
    {
        public static DataFolder GetRootFolder(string instance)
        {
            Require.NotNullOrEmpty(instance, "instance");
            return DataFolders.Instances.GetFolder(instance).GetFolder("Multilingual", DataFileFormats.Json);
        }

        public static DataFolder GetLanguagesFolder(string instance)
        {
            return GetRootFolder(instance).GetFolder("Languages");
        }

        public static DataFolder GetLanguageFolder(string instance, string code)
        {
            return GetLanguagesFolder(instance).GetFolder(code);
        }
    }
}