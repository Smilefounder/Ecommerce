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
    public interface IModelParameterProvider
    {
        /// <summary>
        /// Get the available condition expression parameters for the specified model type.
        /// </summary>
        IEnumerable<ParameterInfo> GetParameters(Type modelType);
    }

    [Dependency(typeof(IModelParameterProvider))]
    public class DefaultModelParameterProvider: IModelParameterProvider
    {
        private IEngine _engine;
        private Lazy<List<IParameter>> _staticParameters;

        public DefaultModelParameterProvider()
            : this(EngineContext.Current)
        {
        }

        public DefaultModelParameterProvider(IEngine engine)
        {
            _engine = engine;
            _staticParameters = new Lazy<List<IParameter>>(ReloadStaticParameters, true);
        }

        public IEnumerable<ParameterInfo> GetParameters(Type modelType)
        {
            return GetAvailableParametersRecursive(modelType, Enumerable.Empty<MemberInfo>(), new HashSet<Type>());
        }

        private IEnumerable<ParameterInfo> GetAvailableParametersRecursive(Type modelType, IEnumerable<MemberInfo> modelPath, HashSet<Type> inspectedModelTypes)
        {
            if (inspectedModelTypes.Contains(modelType))
            {
                return Enumerable.Empty<ParameterInfo>();
            }

            var parameters = new List<ParameterInfo>();

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

        private IEnumerable<ParameterInfo> GetAvailableParametersNoRecursive(Type modelType, IEnumerable<MemberInfo> modelPath)
        {
            var parameters = new List<ParameterInfo>();

            foreach (var param in _staticParameters.Value.Where(x => x.ModelType == modelType))
            {
                parameters.Add(new ParameterInfo(param, modelPath));
            }

            foreach (var prop in modelType.GetProperties(BindingFlags.Public | BindingFlags.Instance))
            {
                var param = DynamicParameter.TryCreateFrom(prop);
                if (param != null)
                {
                    parameters.Add(new ParameterInfo(param, modelPath));
                }
            }

            return parameters;
        }

        private List<IParameter> ReloadStaticParameters()
        {
            return _engine.ResolveAll<IParameter>().ToList();
        }
    }
}
