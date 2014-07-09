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

        public IEnumerable<EQ> GetExtendedQueries<EQ>()
            where EQ : IExtendedQuery
        {
            return EngineContext.Current.ResolveAll<EQ>();
        }

        public EQ GetExtendedQuery<EQ>(string name)
            where EQ : IExtendedQuery
        {
            var queries = GetExtendedQueries<EQ>();
            return queries.FirstOrDefault(o => o.Name == name);
        }

        public IEnumerable<ExtendedQueryParameter> GetExtendedQueryParameters<EQ>(string name)
            where EQ : IExtendedQuery
        {
            var folder = string.Format("{0}/{1}", typeof(EQ).Name, name);
            IEnumerable<ExtendedQueryParameter> paras = _persistence.GetObject<IEnumerable<ExtendedQueryParameter>>(folder);
            if(paras == null)
            {
                var query = GetExtendedQuery<EQ>(name);
                paras = query.Parameters;
            }
            foreach(var para in paras)
            {
                if (para.Value == null)
                    para.Value = para.DefaultValue;
            }
            return paras;
        }

        public void SaveExtendedQueryParameters<EQ>(string name, IEnumerable<ExtendedQueryParameter> parameters)
            where EQ : IExtendedQuery
        {
            var folder = string.Format("{0}/{1}", typeof(EQ).Name, name);
            _persistence.SaveObject<IEnumerable<ExtendedQueryParameter>>(folder, parameters);
        }
    }
}
