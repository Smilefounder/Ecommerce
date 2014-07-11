using Kooboo.Commerce.Data;
using Kooboo.Commerce.Data.Folders;
using Kooboo.Web.Url;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.Web.Framework.Queries
{
    public class QueryInfo
    {
        private DataFile _configFile;

        public IQuery Query { get; private set; }

        public QueryInfo(IQuery query, QueryType queryType, DataFolderFactory folderFactory)
        {
            Query = query;

            var folderName = queryType.DisplayName;
            var virtualPath = UrlUtility.Combine(CommerceDataFolderVirtualPaths.Shared, "Queries", folderName, query.Name + ".config");
            _configFile = folderFactory.GetFile(virtualPath, DataFileFormats.Json);
        }

        public string GetDisplayName()
        {
            var data = _configFile.Read<QuerySettings>();
            return data == null ? Query.DisplayName : data.DisplayName;
        }

        public void SetDisplayName(string displayName)
        {
            var data = _configFile.Read<QuerySettings>() ?? new QuerySettings();
            data.DisplayName = displayName;
            _configFile.Write(data);
        }

        public object GetQueryConfig()
        {
            var data = _configFile.Read<QuerySettings>();
            return data == null ? null : data.ConfigData;
        }

        public void SetQueryConfig(object config)
        {
            var data = _configFile.Read<QuerySettings>() ?? new QuerySettings();
            data.ConfigData = config;
            _configFile.Write(data);
        }

        class QuerySettings
        {
            public string DisplayName { get; set; }

            public object ConfigData { get; set; }
        }
    }

}
