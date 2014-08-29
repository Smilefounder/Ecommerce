using Kooboo.Commerce.Data.Folders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Kooboo.Commerce.Recommendations.Engine
{
    public static class RecommendationsDataFolder
    {
        public static DataFolder For(string instance)
        {
            return DataFolders.Instances.GetFolder(instance).GetFolder("Recommendations", DataFileFormats.Json);
        }
    }
}