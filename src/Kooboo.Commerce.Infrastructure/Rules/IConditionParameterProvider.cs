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
    /// <summary>
    /// Provides available condition expression parameters for a model type.
    /// </summary>
    public interface IConditionParameterProvider
    {
        /// <summary>
        /// Get the available condition expression parameters for the specified model type.
        /// </summary>
        IEnumerable<IConditionParameter> GetParameters(Type contextModelType);
    }

    [Dependency(typeof(IConditionParameterProvider), Key = "DefaultConditionParameterProvider")]
    public class DefaultConditionParameterProvider : IConditionParameterProvider
    {
        private IEngine _engine;
        private Lazy<List<IConditionParameter>> _staticParameters;

        public DefaultConditionParameterProvider()
            : this(EngineContext.Current)
        {
        }

        public DefaultConditionParameterProvider(IEngine engine)
        {
            _engine = engine;
            _staticParameters = new Lazy<List<IConditionParameter>>(ReloadStaticParameters, true);
        }

        public IEnumerable<IConditionParameter> GetParameters(Type contextModelType)
        {
            return GetAvailableParametersRecursive(contextModelType, Enumerable.Empty<MemberInfo>(), new HashSet<Type>());
        }

        private IEnumerable<IConditionParameter> GetAvailableParametersRecursive(Type contextModelType, IEnumerable<MemberInfo> modelPath, HashSet<Type> inspectedModelTypes)
        {
            if (inspectedModelTypes.Contains(contextModelType))
            {
                return Enumerable.Empty<IConditionParameter>();
            }

            var parameters = new List<IConditionParameter>();

            foreach (var param in GetAvailableParametersNoRecursive(contextModelType, modelPath))
            {
                parameters.Add(param);
            }

            inspectedModelTypes.Add(contextModelType);

            foreach (var prop in contextModelType.GetProperties(BindingFlags.Public | BindingFlags.Instance).Where(MaybeParameterModel))
            {
                var path = modelPath.Union(new[] { prop }).ToList();
                parameters.AddRange(GetAvailableParametersRecursive(prop.PropertyType, path, inspectedModelTypes));
            }

            return parameters;
        }

        private bool MaybeParameterModel(PropertyInfo property)
        {
            var propType = property.PropertyType;
            return propType.IsClass && !typeof(IEnumerable).IsAssignableFrom(propType);
        }

        private IEnumerable<IConditionParameter> GetAvailableParametersNoRecursive(Type contextModelType, IEnumerable<MemberInfo> modelPathFromRoot)
        {
            var parameters = new List<IConditionParameter>();

            foreach (var param in _staticParameters.Value.Where(x => x.ContextModelType == contextModelType))
            {
                parameters.Add(new AdaptedConditionParameter(contextModelType, param, new NestedModelAdapter(modelPathFromRoot)));
            }

            foreach (var prop in contextModelType.GetProperties(BindingFlags.Public | BindingFlags.Instance))
            {
                if (prop.IsDefined(typeof(ConditionParameterAttribute), true))
                {
                    var param = new ModelPropertyBackedConditionParameter(prop.ReflectedType, prop);
                    parameters.Add(new AdaptedConditionParameter(contextModelType, param, new NestedModelAdapter(modelPathFromRoot)));
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
