using Kooboo.Commerce.Data;
using Kooboo.Commerce.Data.Folders;
using Kooboo.Web.Url;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.Web.Framework.Queries
{
    public class QueryManager
    {
        public static QueryManager Instance = new QueryManager();

        readonly DataFolder _folder = new DataFolder(UrlUtility.Combine(CommerceDataFolderVirtualPaths.Shared, "Queries"), JsonDataFileFormat.Instance);
        readonly Dictionary<string, QueryInfo> _queriesByNames = new Dictionary<string, QueryInfo>(StringComparer.OrdinalIgnoreCase);
        readonly Dictionary<Type, List<QueryInfo>> _queriesByContractTypes = new Dictionary<Type, List<QueryInfo>>();

        public IEnumerable<QueryInfo> GetQueryInfos(QueryType type)
        {
            List<QueryInfo> queries;
            if (_queriesByContractTypes.TryGetValue(type.Type, out queries))
            {
                return queries;
            }

            return Enumerable.Empty<QueryInfo>();
        }

        public QueryInfo GetQueryInfo(string name)
        {
            QueryInfo info;
            if (_queriesByNames.TryGetValue(name, out info))
            {
                return info;
            }
            return null;
        }

        public void Register(IQuery query)
        {
            if (_queriesByNames.ContainsKey(query.Name))
                throw new InvalidOperationException("An query named '" + query.Name + "' already exists.");

            var queryType = QueryType.Of(query);

            if (!_queriesByContractTypes.ContainsKey(queryType.Type))
            {
                _queriesByContractTypes.Add(queryType.Type, new List<QueryInfo>());
            }

            var queryInfo = new QueryInfo(query, queryType);

            _queriesByContractTypes[queryType.Type].Add(queryInfo);
            _queriesByNames.Add(query.Name, queryInfo);
        }
    }
}
