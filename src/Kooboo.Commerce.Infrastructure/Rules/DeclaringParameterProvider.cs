using Kooboo.CMS.Common.Runtime;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Kooboo.Commerce.Rules
{
    /// <summary>
    /// A provider providing parameters by checking the declared <see cref="Kooboo.Commerce.Rules.ParamAttribute"/> in the class properties.
    /// </summary>
    public class DeclaringParameterProvider : IParameterProvider
    {
        public Func<Type, object> ActivateType = type => EngineContext.Current.Resolve(type);

        public IEnumerable<ConditionParameter> GetParameters(Type dataContextType)
        {
            Require.NotNull(dataContextType, "dataContextType");
            return FindConditionParameters(dataContextType, ParameterValueResolver.Dumb(), null);
        }

        /// <summary>
        /// Gets the available parameters for the specified data context type, 
        /// with a specific data context adapter to adapt original data context to the final data context to which the parameters is applicable.
        /// </summary>
        /// <param name="dataContextType">The data context type.</param>
        /// <param name="dataContextAdapter">The adapter to adapt original data context to the final data context.</param>
        /// <returns>Available parameters.</returns>
        public IEnumerable<ConditionParameter> GetParameters(Type dataContextType, Func<object, object> dataContextAdapter)
        {
            Require.NotNull(dataContextType, "dataContextType");

            var containerResolver = ParameterValueResolver.Dumb();
            if (dataContextAdapter != null)
            {
                Func<ConditionParameter, object, object> resolver = (param, dataContext) => dataContextAdapter(dataContext);
                containerResolver = ParameterValueResolver.FromDelegate(resolver);
            }

            return FindConditionParameters(dataContextType, containerResolver, null);
        }

        private List<ConditionParameter> FindConditionParameters(Type containerType, ParameterValueResolver containerResolver, string prefix)
        {
            var parameters = new List<ConditionParameter>();

            foreach (var property in containerType.GetProperties(BindingFlags.Public | BindingFlags.Instance))
            {
                var refAttr = property.GetCustomAttribute<ReferenceAttribute>(false);
                if (refAttr != null)
                {
                    var resolver = new ChainedParameterValueResolver().Chain(containerResolver)
                                                                      .Chain(new PropertyBackedParameterValueResolver(property));

                    var referencingType = refAttr.ReferencingType ?? property.PropertyType;
                    // Indirect reference
                    if (referencingType != property.PropertyType)
                    {
                        if (refAttr.ReferenceResolver == null)
                            throw new InvalidOperationException("Indirect reference must speicify a reference resolver. Property: " + property.ReflectedType.FullName + "." + property.Name + ".");

                        resolver.Chain(new IndirectReferenceAdapter(referencingType, refAttr.ReferenceResolver));
                    }

                    var newPrefix = prefix;

                    // refAttr.Prefix == null: Generate a default prefix
                    // refAttr.Prefix == String.Empty: Do not use prefix
                    // otherwise, use the specified prefix
                    if (refAttr.Prefix != null)
                    {
                        if (refAttr.Prefix.Length > 0)
                        {
                            newPrefix = prefix + (refAttr.Prefix.EndsWith(".") ? refAttr.Prefix : refAttr.Prefix + ".");
                        }
                    }
                    else
                    {
                        // Try to get a smart default preifx if developer doesn't specify one.
                        // If the property name is like BrandId,
                        // then we use 'Brand' instead of 'BrandId'.
                        // So when we are investigating Brand.Name property, we can get a better parameter name 'BrandName' instead of 'BrandIdName'
                        if (property.Name.EndsWith("Id"))
                        {
                            newPrefix = prefix + property.Name.Substring(0, property.Name.Length - 2);
                        }
                        else
                        {
                            newPrefix = prefix + property.Name;
                        }

                        newPrefix = newPrefix + ".";
                    }

                    parameters.AddRange(FindConditionParameters(referencingType, resolver, newPrefix));
                }
                else
                {
                    var paramAttr = property.GetCustomAttribute<ParamAttribute>(false);
                    if (paramAttr != null)
                    {
                        parameters.Add(CreateConditionParameter(containerType, containerResolver, prefix, property, paramAttr));
                    }
                }
            }

            return parameters;
        }

        private ConditionParameter CreateConditionParameter(
            Type containerType, ParameterValueResolver containerResolver, string prefix, PropertyInfo property, ParamAttribute paramAttr)
        {
            var valueType = property.PropertyType;
            var valueResolver = new ChainedParameterValueResolver().Chain(containerResolver)
                                                                   .Chain(new PropertyBackedParameterValueResolver(property));
            var supportedOperators = GetDefaultOperators(valueType);
            IParameterValueSource valueSource = null;

            if (paramAttr.ValueSource != null)
            {
                valueSource = ActivateType(paramAttr.ValueSource) as IParameterValueSource;
            }
            else if (property.PropertyType.IsEnum)
            {
                valueType = typeof(String);
                valueSource = StaticParameterValueSource.FromEnum(property.PropertyType);
            }

            var paramName = prefix + (paramAttr.Name ?? property.Name);

            return new ConditionParameter(paramName, valueType, valueResolver, valueSource, supportedOperators);
        }

        private List<IComparisonOperator> GetDefaultOperators(Type valueType)
        {
            if (valueType == typeof(String))
            {
                return new List<IComparisonOperator>
                {
                    ComparisonOperators.Equals,
                    ComparisonOperators.NotEquals,
                    ComparisonOperators.Contains,
                    ComparisonOperators.NotContains
                };
            }
            else if (valueType.IsEnum)
            {
                return new List<IComparisonOperator>
                {
                    ComparisonOperators.Equals,
                    ComparisonOperators.NotEquals
                };
            }
            else if (valueType.IsNumber())
            {
                return new List<IComparisonOperator>
                {
                    ComparisonOperators.Equals,
                    ComparisonOperators.NotEquals,
                    ComparisonOperators.LessThan,
                    ComparisonOperators.LessThanOrEqual,
                    ComparisonOperators.GreaterThan,
                    ComparisonOperators.GreaterThanOrEqual
                };
            }

            return new List<IComparisonOperator>();
        }
    }
}
