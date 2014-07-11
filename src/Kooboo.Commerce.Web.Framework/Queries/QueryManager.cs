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
        readonly Dictionary<string, IQuery> _queriesByNames = new Dictionary<string, IQuery>(StringComparer.OrdinalIgnoreCase);
        readonly Dictionary<Type, List<IQuery>> _queriesByContractTypes = new Dictionary<Type, List<IQuery>>();

        public IEnumerable<IQuery> GetQueries(Type contractType)
        {
            List<IQuery> queries;
            if (_queriesByContractTypes.TryGetValue(contractType, out queries))
            {
                return queries;
            }

            return Enumerable.Empty<IQuery>();
        }

        public IQuery GetQuery(string name)
        {
            IQuery query;
            if (_queriesByNames.TryGetValue(name, out query))
            {
                return query;
            }

            return null;
        }

        public void Register(Type contractType, IQuery query)
        {
            if (_queriesByNames.ContainsKey(query.Name))
                throw new InvalidOperationException("An query named '" + query.Name + "' already exists.");

            if (!_queriesByContractTypes.ContainsKey(contractType))
            {
                _queriesByContractTypes.Add(contractType, new List<IQuery>());
            }

            _queriesByContractTypes[contractType].Add(query);
            _queriesByNames.Add(query.Name, query);
        }

        public object GetQueryConfig(string queryName)
        {
            var query = GetQuery(queryName);
            if (query.ConfigType == null)
            {
                return null;
            }

            var file = _folder.GetFile(queryName + ".config");
            if (file.Exists)
            {
                return file.Read(query.ConfigType);
            }

            return null;
        }

        public void SaveQueryConfig(string queryName, object config)
        {
            var file = _folder.GetFile(queryName + ".config");
            file.Write(config);
        }
    }
}
