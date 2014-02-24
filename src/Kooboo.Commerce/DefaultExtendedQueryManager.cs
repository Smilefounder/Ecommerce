using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Kooboo.CMS.Common.Runtime;
using Kooboo.CMS.Common.Runtime.Dependency;

namespace Kooboo.Commerce
{
    [Dependency(typeof(IExtendedQueryManager), ComponentLifeStyle.Singleton)]
    public class DefaultExtendedQueryManager : IExtendedQueryManager
    {
        private IObjectPersistence _persistence;

        public DefaultExtendedQueryManager(IObjectPersistence persistence)
        {
            _persistence = persistence;
        }

        public IEnumerable<IExtendedQuery<T>> GetExtendedQueries<T>()
            where T : class, new()
        {
            return EngineContext.Current.ResolveAll<IExtendedQuery<T>>();
        }

        public IExtendedQuery<T> GetExtendedQuery<T>(string name)
            where T : class, new()
        {
            var queries = GetExtendedQueries<T>();
            return queries.FirstOrDefault(o => o.Name == name);
        }

        public IEnumerable<ExtendedQueryParameter> GetExtendedQueryParameters<T>(string name)
            where T : class, new()
        {
            var folder = string.Format("{0}/{1}", typeof(T).Name, name);
            IEnumerable<ExtendedQueryParameter> paras = _persistence.GetObject<IEnumerable<ExtendedQueryParameter>>(folder);
            if(paras == null)
            {
                var query = GetExtendedQuery<T>(name);
                paras = query.Parameters;
            }
            foreach(var para in paras)
            {
                if (para.Value == null)
                    para.Value = para.DefaultValue;
            }
            return paras;
        }

        public void SaveExtendedQueryParameters<T>(string name, IEnumerable<ExtendedQueryParameter> parameters)
            where T : class, new()
        {
            var folder = string.Format("{0}/{1}", typeof(T).Name, name);
            _persistence.SaveObject<IEnumerable<ExtendedQueryParameter>>(folder, parameters);
        }
    }
}
