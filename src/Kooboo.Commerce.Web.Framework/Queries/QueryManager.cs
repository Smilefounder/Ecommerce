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

        readonly IObjectPersistence _persistence = new JsonObjectPersistence();
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

        public IQuery GetQuery(Type contractType, string name)
        {
            return GetQueries(contractType).FirstOrDefault(q => q.Name == name);
        }

        public void Register(Type contractType, IQuery query)
        {
            if (!_queriesByContractTypes.ContainsKey(contractType))
            {
                _queriesByContractTypes.Add(contractType, new List<IQuery>());
            }

            _queriesByContractTypes[contractType].Add(query);
        }

        public object GetQueryConfig(Type contractType, string name)
        {
            var query = GetQuery(contractType, name);
            if (query.ConfigModelType == null)
            {
                return null;
            }

            var folder = string.Format("{0}/{1}", query.GetType().Name, name);
            object config = null;
            var json = _persistence.GetObject<string>(folder);
            if (String.IsNullOrEmpty(json))
            {
                config = TypeActivator.CreateInstance(query.ConfigModelType);
            }
            else
            {
                config = JsonConvert.DeserializeObject(json, query.ConfigModelType);
            }

            return config;
        }

        public void SaveQueryConfig(Type contractType, string name, object parameters)
        {
            var query = GetQuery(contractType, name);
            var folder = string.Format("{0}/{1}", query.GetType().Name, name);
            _persistence.SaveObject<string>(folder, JsonConvert.SerializeObject(parameters));
        }
    }
}
