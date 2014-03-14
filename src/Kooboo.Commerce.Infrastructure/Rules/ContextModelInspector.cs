using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Kooboo.Commerce.Rules
{
    public class ContextModelInspector
    {
        private IConditionParameterFactory _parameters;

        public ContextModelInspector(IConditionParameterFactory parameterFactory)
        {
            _parameters = parameterFactory;
        }

        public IEnumerable<ModelParameter> GetAvailableParameters(Type modelType)
        {
            return GetAvailableParametersRecursive(modelType, new HashSet<Type>());
        }

        private IEnumerable<ModelParameter> GetAvailableParametersRecursive(Type modelType, HashSet<Type> inspectedModelTypes)
        {
            if (inspectedModelTypes.Contains(modelType))
            {
                return Enumerable.Empty<ModelParameter>();
            }

            var parameters = new List<ModelParameter>();

            foreach (var param in _parameters.FindByModelType(modelType))
            {
                parameters.Add(new ModelParameter(param, modelType));
            }

            inspectedModelTypes.Add(modelType);

            foreach (var prop in modelType.GetProperties(BindingFlags.Public | BindingFlags.Instance).Where(MaybeParameterModel))
            {
                parameters.AddRange(GetAvailableParametersRecursive(prop.PropertyType, inspectedModelTypes));
            }

            return parameters;
        }

        private bool MaybeParameterModel(PropertyInfo prop)
        {
            var propType = prop.PropertyType;
            return propType.IsClass && !typeof(IEnumerable).IsAssignableFrom(propType);
        }
    }

    public class ModelParameter
    {
        public IConditionParameter Parameter { get; set; }

        public MemberInfo Path { get; set; }

        public ModelParameter(IConditionParameter parameter, MemberInfo path)
        {
            Parameter = parameter;
            Path = path;
        }
    }
}
