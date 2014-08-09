using Kooboo.Commerce.Data.Folders;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;

namespace Kooboo.Commerce.Search.Rebuild
{
    public class FileBasedRebuildInfoManager : IRebuildInfoManager
    {
        public RebuildInfo Load(IndexKey indexKey)
        {
            return GetDataFile(indexKey).Read<RebuildInfo>();
        }

        public void Save(IndexKey indexKey, RebuildInfo info)
        {
            GetDataFile(indexKey).Write(info);
        }

        private DataFile GetDataFile(IndexKey key)
        {
            return DataFolders.Instances.GetFolder(key.Instance)
                                        .GetFolder("Indexes")
                                        .GetFolder("RebuildInfos", DataFileFormats.Json)
                                        .GetFile(key.ModelType.Name + (String.IsNullOrEmpty(key.Culture.Name) ? String.Empty : "-" + key.Culture.Name) + ".json");
        }
    }
}