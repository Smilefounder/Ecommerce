using Kooboo.CMS.Common.Runtime;
using Kooboo.CMS.Common.Runtime.Dependency;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.Rules
{
    public interface IComparisonOperatorProvider
    {
        IEnumerable<IComparisonOperator> GetAllOperators();

        IComparisonOperator GetOperatorByName(string name);
    }

    [Dependency(typeof(IComparisonOperatorProvider), ComponentLifeStyle.Singleton)]
    public class DefaultComparisonOperatorProvider : IComparisonOperatorProvider
    {
        private IEngine _engine;
        private Lazy<List<IComparisonOperator>> _operators;

        public DefaultComparisonOperatorProvider()
            : this(EngineContext.Current)
        {
        }

        public DefaultComparisonOperatorProvider(IEngine engine)
        {
            _engine = engine;
            _operators = new Lazy<List<IComparisonOperator>>(Reload, true);
        }

        public IEnumerable<IComparisonOperator> GetAllOperators()
        {
            return _operators.Value;
        }

        public IComparisonOperator GetOperatorByName(string name)
        {
            return _operators.Value.FirstOrDefault(x => x.Name == name);
        }

        private List<IComparisonOperator> Reload()
        {
            return _engine.ResolveAll<IComparisonOperator>().ToList();
        }
    }
}
