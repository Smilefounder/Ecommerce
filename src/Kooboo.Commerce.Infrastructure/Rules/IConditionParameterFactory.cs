using Kooboo.CMS.Common.Runtime;
using Kooboo.CMS.Common.Runtime.Dependency;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Kooboo.Commerce.Rules
{
    public interface IConditionParameterFactory
    {
        IEnumerable<ConditionParameterInfo> GetConditionParameterInfos(Type modelType);
    }

    [Dependency(typeof(IConditionParameterFactory))]
    public class DefaultConditionParameterFactory : IConditionParameterFactory
    {
        private IEngine _engine;
        private Lazy<List<IConditionParameter>> _staticParameters;

        public DefaultConditionParameterFactory()
            : this(EngineContext.Current)
        {
        }

        public DefaultConditionParameterFactory(IEngine engine)
        {
            _engine = engine;
            _staticParameters = new Lazy<List<IConditionParameter>>(ReloadStaticParameters, true);
        }

        public IEnumerable<ConditionParameterInfo> GetConditionParameterInfos(Type modelType)
        {
            return GetAvailableParametersRecursive(modelType, Enumerable.Empty<MemberInfo>(), new HashSet<Type>());
        }

        private IEnumerable<ConditionParameterInfo> GetAvailableParametersRecursive(Type modelType, IEnumerable<MemberInfo> modelPath, HashSet<Type> inspectedModelTypes)
        {
            if (inspectedModelTypes.Contains(modelType))
            {
                return Enumerable.Empty<ConditionParameterInfo>();
            }

            var parameters = new List<ConditionParameterInfo>();

            foreach (var param in GetAvailableParametersNoRecursive(modelType, modelPath))
            {
                parameters.Add(param);
            }

            inspectedModelTypes.Add(modelType);

            foreach (var prop in modelType.GetProperties(BindingFlags.Public | BindingFlags.Instance).Where(MaybeParameterModel))
            {
                var path = modelPath.Union(new[] { prop }).ToList();
                parameters.AddRange(GetAvailableParametersRecursive(prop.PropertyType, path, inspectedModelTypes));
            }

            return parameters;
        }

        private bool MaybeParameterModel(PropertyInfo prop)
        {
            var propType = prop.PropertyType;
            return propType.IsClass && !typeof(IEnumerable).IsAssignableFrom(propType);
        }

        private IEnumerable<ConditionParameterInfo> GetAvailableParametersNoRecursive(Type modelType, IEnumerable<MemberInfo> modelPath)
        {
            var parameters = new List<ConditionParameterInfo>();

            foreach (var param in _staticParameters.Value.Where(x => x.ModelType == modelType))
            {
                parameters.Add(new ConditionParameterInfo(param, modelPath));
            }

            foreach (var prop in modelType.GetProperties(BindingFlags.Public | BindingFlags.Instance))
            {
                var param = DynamicConditionParameter.TryCreateFrom(prop);
                if (param != null)
                {
                    parameters.Add(new ConditionParameterInfo(param, modelPath));
                }
            }

            return parameters;
        }

        private List<IConditionParameter> ReloadStaticParameters()
        {
            return _engine.ResolveAll<IConditionParameter>().ToList();
        }
    }
}
