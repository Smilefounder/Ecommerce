using Kooboo.CMS.Common.Runtime;
using Kooboo.CMS.Common.Runtime.Dependency;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Kooboo.Commerce.Rules
{
    [Dependency(typeof(IConditionParameterProvider), Key = "DeclaringConditionParameterProvider")]
    public class DeclaringConditionParameterProvider : IConditionParameterProvider
    {
        public IEnumerable<ConditionParameter> GetParameters(Type dataContextType)
        {
            return FindConditionParameters(dataContextType, DumbParameterValueResolver.Instance, null);
        }

        private List<ConditionParameter> FindConditionParameters(Type containerType, IParameterValueResolver containerResolver, string prefix)
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

                        resolver.Chain(new IndirectReferenceParameterValueResolverAdapter(referencingType, refAttr.ReferenceResolver));
                    }

                    var newPrefix = prefix;
                    if (refAttr.Prefix != null)
                    {
                        newPrefix = prefix + refAttr.Prefix;
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
            Type containerType, IParameterValueResolver containerResolver, string prefix, PropertyInfo property, ParamAttribute paramAttr)
        {
            var valueType = property.PropertyType;
            var valueResolver = new ChainedParameterValueResolver().Chain(containerResolver)
                                                                   .Chain(new PropertyBackedParameterValueResolver(property));
            var supportedOperators = GetDefaultOperators(valueType);
            IParameterValueSource valueSource = null;

            if (paramAttr.ValueSource != null)
            {
                valueSource = EngineContext.Current.Resolve(paramAttr.ValueSource) as IParameterValueSource;
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
                    ComparisonOperators.NotContains,
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
