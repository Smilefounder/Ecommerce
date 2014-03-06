using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Kooboo.Commerce.Rules
{
    public interface IModelInspector
    {
        IEnumerable<ModelParameter> GetParameters(Type modelType);
    }

    public class ModelInspector : IModelInspector
    {
        private IConditionParameterFactory _registry;

        public ModelInspector(IConditionParameterFactory register)
        {
            _registry = register;
        }

        public IEnumerable<ModelParameter> GetParameters(Type modelType)
        {
            var parameters = new List<ModelParameter>();
            foreach (var param in _registry.FindByModelType(modelType))
            {
                parameters.Add(new ModelParameter(param, modelType));
            }

            foreach (var prop in modelType.GetProperties(BindingFlags.Public | BindingFlags.Instance))
            {
                var propType = prop.PropertyType;
                if (!propType.IsPrimitive 
                    && !propType.IsValueType
                    && propType != typeof(string))
                {
                    foreach (var param in _registry.FindByModelType(prop.PropertyType))
                    {
                        parameters.Add(new ModelParameter(param, propType));
                    }
                }
            }

            return parameters;
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
