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

        public IEnumerable<IExtendedQuery<T, Q>> GetExtendedQueries<T, Q>()
            where T : class, new()
            where Q : class, new()
        {
            return EngineContext.Current.ResolveAll<IExtendedQuery<T, Q>>();
        }

        public IExtendedQuery<T, Q> GetExtendedQuery<T, Q>(string name)
            where T : class, new()
            where Q : class, new()
        {
            var queries = GetExtendedQueries<T, Q>();
            return queries.FirstOrDefault(o => o.Name == name);
        }

        public IEnumerable<ExtendedQueryParameter> GetExtendedQueryParameters<T, Q>(string name)
            where T : class, new()
            where Q : class, new()
        {
            var folder = string.Format("{0}/{1}", typeof(T).Name, name);
            IEnumerable<ExtendedQueryParameter> paras = _persistence.GetObject<IEnumerable<ExtendedQueryParameter>>(folder);
            if(paras == null)
            {
                var query = GetExtendedQuery<T, Q>(name);
                paras = query.Parameters;
            }
            foreach(var para in paras)
            {
                if (para.Value == null)
                    para.Value = para.DefaultValue;
            }
            return paras;
        }

        public void SaveExtendedQueryParameters<T, Q>(string name, IEnumerable<ExtendedQueryParameter> parameters)
            where T : class, new()
            where Q : class, new()
        {
            var folder = string.Format("{0}/{1}", typeof(T).Name, name);
            _persistence.SaveObject<IEnumerable<ExtendedQueryParameter>>(folder, parameters);
        }
    }
}
