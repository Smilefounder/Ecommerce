using Kooboo.CMS.Common.Runtime;
using Kooboo.CMS.Common.Runtime.Dependency;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Kooboo.Commerce.Rules
{
    public interface IConditionParameterFactory
    {
        IEnumerable<IConditionParameter> All();

        IConditionParameter FindByName(string paramName);

        IEnumerable<IConditionParameter> FindByModelType(Type modelType);
    }

    [Dependency(typeof(IConditionParameterFactory))]
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
            var parameters = _parameters.Value.Where(x => x.ModelType == modelType).ToList();

            foreach (var prop in modelType.GetProperties(BindingFlags.Public | BindingFlags.Instance))
            {
                var param = DynamicConditionParameter.TryCreateFrom(prop);
                if (param != null)
                {
                    parameters.Add(param);
                }
            }

            return parameters;
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
