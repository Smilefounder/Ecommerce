using Kooboo.CMS.Common.Runtime;
using Kooboo.Commerce.Rules.Conditions.Operators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Kooboo.Commerce.Rules.Parameters
{
    /// <summary>
    /// A provider providing parameters by checking the declared <see cref="Kooboo.Commerce.Rules.ParamAttribute"/> in the class properties.
    /// </summary>
    public class DefaultRuleParameterProvider : IRuleParameterProvider
    {
        private RuleParameterProviderCollection _providers;

        protected internal RuleParameterProviderCollection Providers
        {
            get
            {
                if (_providers == null)
                {
                    _providers = RuleParameterProviders.Providers;
                }
                return _providers;
            }
            set
            {
                _providers = value;
            }
        }

        public IEnumerable<RuleParameter> GetParameters(Type dataContextType)
        {
            Require.NotNull(dataContextType, "dataContextType");
            return FindConditionParameters(dataContextType, RuleParameterValueResolver.Dumb(), null);
        }

        /// <summary>
        /// Gets the available parameters for the specified data context type, 
        /// with a specific data context adapter to adapt original data context to the final data context to which the parameters is applicable.
        /// </summary>
        /// <param name="dataContextType">The data context type.</param>
        /// <param name="dataContextAdapter">The adapter to adapt original data context to the final data context.</param>
        /// <returns>Available parameters.</returns>
        public IEnumerable<RuleParameter> GetParameters(Type dataContextType, Func<object, object> dataContextAdapter)
        {
            Require.NotNull(dataContextType, "dataContextType");

            var containerResolver = RuleParameterValueResolver.Dumb();
            if (dataContextAdapter != null)
            {
                Func<RuleParameter, object, object> resolver = (param, dataContext) => dataContextAdapter(dataContext);
                containerResolver = RuleParameterValueResolver.FromDelegate(resolver);
            }

            return FindConditionParameters(dataContextType, containerResolver, null);
        }

        private List<RuleParameter> FindConditionParameters(Type containerType, RuleParameterValueResolver containerResolver, string prefix)
        {
            var parameters = new List<RuleParameter>();

            foreach (var property in containerType.GetProperties(BindingFlags.Public | BindingFlags.Instance))
            {
                var refAttr = property.GetCustomAttribute<ReferenceAttribute>(false);
                if (refAttr != null)
                {
                    var resolver = new ChainedRuleParameterValueResolver().Chain(containerResolver, new PropertyBackedRuleParameterValueResolver(property));

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

                    // Add also extended parameters
                    foreach (var provider in Providers)
                    {
                        if (provider.GetType() != typeof(DefaultRuleParameterProvider))
                        {
                            var additionalParams = provider.GetParameters(referencingType);
                            foreach (var param in additionalParams)
                            {
                                var valueResolver = new ChainedRuleParameterValueResolver();
                                valueResolver.Chain(resolver);
                                valueResolver.Chain(param.ValueResolver);

                                var adaptedParam = new RuleParameter(newPrefix + param.Name, param.ValueType, valueResolver, param.SupportedOperators)
                                {
                                    ValueSource = param.ValueSource
                                };

                                parameters.Add(adaptedParam);
                            }
                        }
                    }
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

        private RuleParameter CreateConditionParameter(
            Type containerType, RuleParameterValueResolver containerResolver, string prefix, PropertyInfo property, ParamAttribute paramAttr)
        {
            var propertyType = property.PropertyType;
            var valueResolver = new ChainedRuleParameterValueResolver().Chain(containerResolver, new PropertyBackedRuleParameterValueResolver(property));
            var supportedOperators = GetDefaultOperators(propertyType);
            IRuleParameterValueSource valueSource = null;

            if (paramAttr.ValueSource != null)
            {
                valueSource = TypeActivator.CreateInstance(paramAttr.ValueSource) as IRuleParameterValueSource;
            }
            else if (property.PropertyType.IsEnum)
            {
                propertyType = typeof(String);
                valueSource = StaticRuleParameterValueSource.FromEnum(property.PropertyType);
            }
            else if (IsNullableEnum(property.PropertyType))
            {
                propertyType = typeof(String);
                valueSource = StaticRuleParameterValueSource.FromEnum(Nullable.GetUnderlyingType(property.PropertyType));
            }

            var paramName = prefix + (paramAttr.Name ?? property.Name);

            return new RuleParameter(paramName, propertyType, valueResolver, supportedOperators)
            {
                ValueSource = valueSource
            };
        }

        private List<IComparisonOperator> GetDefaultOperators(Type propertyType)
        {
            if (propertyType == typeof(String))
            {
                return new List<IComparisonOperator>
                {
                    ComparisonOperators.Equals,
                    ComparisonOperators.NotEquals,
                    ComparisonOperators.Contains,
                    ComparisonOperators.NotContains
                };
            }
            else if (propertyType.IsEnum || IsNullableEnum(propertyType))
            {
                return new List<IComparisonOperator>
                {
                    ComparisonOperators.Equals,
                    ComparisonOperators.NotEquals
                };
            }
            else if (propertyType.IsNumericType())
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

        private bool IsNullableEnum(Type type)
        {
            return type.IsValueType
                && type.IsGenericType
                && type.GetGenericTypeDefinition() == typeof(Nullable<>)
                && Nullable.GetUnderlyingType(type).IsEnum;
        }
    }
}
