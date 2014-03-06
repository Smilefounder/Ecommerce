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
        private IParameterFactory _registry;

        public ModelInspector(IParameterFactory register)
        {
            _registry = register;
        }

        public IEnumerable<ModelParameter> GetParameters(Type modelType)
        {
            var parameters = new List<ModelParameter>();
            foreach (var paramType in _registry.GetParameterTypes(modelType))
            {
                parameters.Add(new ModelParameter(ActivateParameter(paramType), modelType));
            }

            foreach (var prop in modelType.GetProperties(BindingFlags.Public | BindingFlags.Instance))
            {
                var propType = prop.PropertyType;
                if (!propType.IsPrimitive 
                    && !propType.IsValueType
                    && propType != typeof(string))
                {
                    foreach (var paramType in _registry.GetParameterTypes(prop.PropertyType))
                    {
                        parameters.Add(new ModelParameter(ActivateParameter(paramType), propType));
                    }
                }
            }

            return parameters;
        }

        private IParameter ActivateParameter(Type parameterType)
        {
            return (IParameter)Activator.CreateInstance(parameterType);
        }
    }

    public interface IParameterFactory
    {
        IEnumerable<Type> GetParameterTypes(Type modelType);

        void RegisterParameters(IEnumerable<Type> parameterTypes);

        void RegisterAssemblies(IEnumerable<Assembly> assemblies);
    }

    public class ModelParameter
    {
        public IParameter Parameter { get; set; }

        public MemberInfo Path { get; set; }

        public ModelParameter(IParameter parameter, MemberInfo path)
        {
            Parameter = parameter;
            Path = path;
        }
    }
}
