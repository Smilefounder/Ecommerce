using Kooboo.CMS.Common.Runtime;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.Rules
{
    public interface IComparisonOperatorFactory
    {
        IEnumerable<IComparisonOperator> All();

        IComparisonOperator FindByName(string name);
    }

    public class DefaultComparisonOperatorFactory : IComparisonOperatorFactory
    {
        private IEngine _engine;
        private Lazy<List<IComparisonOperator>> _operators;

        public DefaultComparisonOperatorFactory()
            : this(EngineContext.Current)
        {
        }

        public DefaultComparisonOperatorFactory(IEngine engine)
        {
            _engine = engine;
            _operators = new Lazy<List<IComparisonOperator>>(Reload, true);
        }

        public IEnumerable<IComparisonOperator> All()
        {
            return _operators.Value;
        }

        public IComparisonOperator FindByName(string name)
        {
            return _operators.Value.FirstOrDefault(x => x.Name == name);
        }

        private List<IComparisonOperator> Reload()
        {
            return _engine.ResolveAll<IComparisonOperator>().ToList();
        }
    }
}
