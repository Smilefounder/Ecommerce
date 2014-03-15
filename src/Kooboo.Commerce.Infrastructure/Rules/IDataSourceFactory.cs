using Kooboo.CMS.Common.Runtime;
using Kooboo.CMS.Common.Runtime.Dependency;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.Rules
{
    public interface IDataSourceFactory
    {
        IEnumerable<IDataSource> All();

        IDataSource FindById(string dataSourceId);

        IEnumerable<IDataSource> FindBySupportedParameter(IConditionParameter param);
    }

    public class DefaultDataSourceFactory : IDataSourceFactory
    {
        private IEngine _engine;

        public DefaultDataSourceFactory()
            : this(EngineContext.Current)
        {
        }

        public DefaultDataSourceFactory(IEngine engine)
        {
            _engine = engine;
        }

        public IEnumerable<IDataSource> All()
        {
            return _engine.ResolveAll<IDataSource>();
        }

        public IDataSource FindById(string dataSourceId)
        {
            return All().FirstOrDefault(x => x.Id == dataSourceId);
        }

        public IEnumerable<IDataSource> FindBySupportedParameter(IConditionParameter param)
        {
            return All().Where(x => x.SupportedParameters.Contains(param.Name)).ToList();
        }
    }
}
