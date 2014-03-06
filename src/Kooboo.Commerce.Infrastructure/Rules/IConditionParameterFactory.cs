using Kooboo.CMS.Common.Runtime;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.Rules
{
    public interface IConditionParameterFactory
    {
        IEnumerable<IConditionParameter> All();

        IConditionParameter FindByName(string paramName);

        IEnumerable<IConditionParameter> FindByModelType(Type modelType);
    }

    public class DefaultConditionParameterFactory : IConditionParameterFactory
    {
        private IEngine _engine;
        private Lazy<List<IConditionParameter>> _parameters;

        public DefaultConditionParameterFactory()
            : this(EngineContext.Current)
        {
        }

        public DefaultConditionParameterFactory(IEngine engine)
        {
            _engine = engine;
            _parameters = new Lazy<List<IConditionParameter>>(Reload, true);
        }

        public IEnumerable<IConditionParameter> All()
        {
            return _parameters.Value;
        }

        public IEnumerable<IConditionParameter> FindByModelType(Type modelType)
        {
            return _parameters.Value.Where(x => x.ModelType == modelType).ToList();
        }

        public IConditionParameter FindByName(string paramName)
        {
            return _parameters.Value.FirstOrDefault(x => x.Name == paramName);
        }

        private List<IConditionParameter> Reload()
        {
            return _engine.ResolveAll<IConditionParameter>().ToList();
        }
    }
}
