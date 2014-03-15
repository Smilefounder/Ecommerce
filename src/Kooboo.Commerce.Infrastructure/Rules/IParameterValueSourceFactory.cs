using Kooboo.CMS.Common.Runtime;
using Kooboo.CMS.Common.Runtime.Dependency;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.Rules
{
    public interface IParameterValueSourceFactory
    {
        IEnumerable<IParameterValueSource> All();

        IParameterValueSource FindById(string dataSourceId);

        IEnumerable<IParameterValueSource> FindByParameter(string paramName);
    }

    [Dependency(typeof(IParameterValueSourceFactory))]
    public class DefaultParameterValueSourceFactory : IParameterValueSourceFactory
    {
        private IEngine _engine;

        public DefaultParameterValueSourceFactory()
            : this(EngineContext.Current)
        {
        }

        public DefaultParameterValueSourceFactory(IEngine engine)
        {
            _engine = engine;
        }

        public IEnumerable<IParameterValueSource> All()
        {
            return _engine.ResolveAll<IParameterValueSource>();
        }

        public IParameterValueSource FindById(string dataSourceId)
        {
            return All().FirstOrDefault(x => x.Id == dataSourceId);
        }

        public IEnumerable<IParameterValueSource> FindByParameter(string paramName)
        {
            return All().Where(x => x.ParameterName == paramName).ToList();
        }
    }
}
