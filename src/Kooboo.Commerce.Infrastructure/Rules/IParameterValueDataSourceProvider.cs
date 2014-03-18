using Kooboo.CMS.Common.Runtime;
using Kooboo.CMS.Common.Runtime.Dependency;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.Rules
{
    public interface IParameterValueDataSourceProvider
    {
        IEnumerable<IParameterValueDataSource> GetDataSources(string paramName);
    }

    [Dependency(typeof(IParameterValueDataSourceProvider))]
    public class DefaultParameterValueDataSourceProvider : IParameterValueDataSourceProvider
    {
        private IEngine _engine;

        public DefaultParameterValueDataSourceProvider()
            : this(EngineContext.Current)
        {
        }

        public DefaultParameterValueDataSourceProvider(IEngine engine)
        {
            _engine = engine;
        }

        public IEnumerable<IParameterValueDataSource> GetDataSources(string paramName)
        {
            return _engine.ResolveAll<IParameterValueDataSource>()
                          .Where(x => x.TargetParameterName == paramName)
                          .ToList();
        }
    }
}
