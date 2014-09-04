using Kooboo.Commerce.Reflection;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Kooboo.Commerce.Api.Local.Mapping
{
    public class DefaultObjectMapper : IObjectMapper
    {
        public Func<Type, Type, IObjectMapper> GetMapper = (sourceType, targetType) => ObjectMapper.GetMapper(sourceType, targetType);

        private IPropertyValueResolver _sourcePropertyValueResolver = new LocalizablePropertyValueResolver();

        public IPropertyValueResolver SourcePropertyValueResolver
        {
            get
            {
                return _sourcePropertyValueResolver;
            }
            set
            {
                Require.NotNull(value, "value");
                _sourcePropertyValueResolver = value;
            }
        }

        public virtual object Map(object source, object target, Type sourceType, Type targetType, string prefix, MappingContext context)
        {
            foreach (var targetProp in targetType.GetProperties(BindingFlags.Public | BindingFlags.Instance))
            {
                var propPath = prefix + targetProp.Name;
                MapProperty(targetProp, source, target, sourceType, targetType, propPath, context);
            }

            return target;
        }

        protected virtual void MapProperty(PropertyInfo targetProperty, object source, object target, Type sourceType, Type targetType, string propertyPath, MappingContext context)
        {
            var sourceProperty = sourceType.GetProperty(targetProperty.Name, BindingFlags.Public | BindingFlags.Instance);
            if (sourceProperty == null)
            {
                return;
            }

            var sourcePropValue = ResolveSourcePropertyValue(sourceProperty, source, context);
            if (sourcePropValue == null)
            {
                if (targetProperty.CanWrite)
                {
                    targetProperty.SetValue(target, null, null);
                }

                return;
            }

            if (IsComplexType(targetProperty.PropertyType))
            {
                if (!IsComplexPropertyIncluded(targetProperty, propertyPath, context))
                {
                    return;
                }

                MapComplexProperty(targetProperty, sourceProperty, sourcePropValue, source, target, sourceType, targetType, propertyPath, context);
            }
            else
            {
                MapSimpleProperty(targetProperty, sourceProperty, sourcePropValue, source, target, sourceType, targetType, propertyPath, context);
            }
        }

        protected virtual bool IsComplexPropertyIncluded(PropertyInfo property, string propertyPath, MappingContext context)
        {
            return context.Includes.Includes(propertyPath);
        }

        protected virtual void MapComplexProperty(PropertyInfo targetProperty, PropertyInfo sourceProperty, object sourcePropValue, object source, object target, Type sourceType, Type targetType, string propertyPath, MappingContext context)
        {
            object targetPropValue = targetProperty.GetValue(target, null);

            if (context.VisitedObjects.Contains(targetPropValue))
            {
                return;
            }

            var targetPropTypeInfo = ModelTypeInfo.GetTypeInfo(targetProperty.PropertyType);

            if (targetPropTypeInfo.IsCollection)
            {
                var sourcePropTypeInfo = ModelTypeInfo.GetTypeInfo(sourceProperty.PropertyType);

                if (targetPropTypeInfo.IsDictionary)
                {
                    if (sourcePropTypeInfo.IsDictionary)
                    {
                        // Map between dictionaries
                        var targetDic = Activator.CreateInstance(typeof(Dictionary<,>).MakeGenericType(targetPropTypeInfo.DictionaryKeyType, targetPropTypeInfo.DictionaryValueType)) as IDictionary;
                        targetProperty.SetValue(target, targetDic, null);

                        var sourceDic = sourcePropValue as IDictionary;
                        foreach (var key in sourceDic.Keys)
                        {
                            targetDic.Add(key, sourceDic[key]);
                        }
                    }
                    else if (sourcePropTypeInfo.IsCollection)
                    {
                        // source collection element: Name or Key field -> dictionary key, Value field -> dictionary value
                        var keyProp = sourcePropTypeInfo.ElementType.GetProperty("Key", BindingFlags.Public | BindingFlags.Instance);
                        if (keyProp == null)
                        {
                            keyProp = sourcePropTypeInfo.ElementType.GetProperty("Name", BindingFlags.Public | BindingFlags.Instance);
                        }

                        if (keyProp != null)
                        {
                            var valueProp = sourcePropTypeInfo.ElementType.GetProperty("Value", BindingFlags.Public | BindingFlags.Instance);
                            if (valueProp != null)
                            {
                                var targetDic = Activator.CreateInstance(typeof(Dictionary<,>).MakeGenericType(targetPropTypeInfo.DictionaryKeyType, targetPropTypeInfo.DictionaryValueType)) as IDictionary;
                                foreach (var item in sourcePropValue as IEnumerable)
                                {
                                    var key = keyProp.GetValue(item, null);
                                    var value = valueProp.GetValue(item, null);
                                    targetDic.Add(key, value);
                                }

                                targetProperty.SetValue(target, targetDic, null);
                            }
                        }
                    }
                }
                else // List or Array
                {
                    var sourceElementType = sourcePropTypeInfo.ElementType;

                    var elementMapper = GetMapperOrDefault(sourceElementType, targetPropTypeInfo.ElementType);
                    if (elementMapper == null)
                    {
                        return;
                    }

                    var targetList = Activator.CreateInstance(typeof(List<>).MakeGenericType(targetPropTypeInfo.ElementType));
                    var addMethod = targetList.GetType().GetMethod("Add", BindingFlags.Public | BindingFlags.Instance, null, new[] { targetPropTypeInfo.ElementType }, null);

                    var sourceList = sourcePropValue as IEnumerable;
                    var elementPrefix = propertyPath + ".";
                    var totalElements = 0;

                    foreach (var sourceElement in sourceList)
                    {
                        var targetElement = elementMapper.Map(sourceElement, Activator.CreateInstance(targetPropTypeInfo.ElementType), sourceElementType, targetPropTypeInfo.ElementType, elementPrefix, context);
                        addMethod.Invoke(targetList, new[] { targetElement });
                        totalElements++;
                    }

                    if (!Object.ReferenceEquals(targetList, targetPropValue))
                    {
                        if (targetProperty.PropertyType.IsArray)
                        {
                            var array = Array.CreateInstance(targetPropTypeInfo.ElementType, totalElements);
                            var i = 0;
                            foreach (var item in targetList as IEnumerable)
                            {
                                array.SetValue(item, i);
                                i++;
                            }

                            targetProperty.SetValue(target, array, null);
                        }
                        else
                        {
                            targetProperty.SetValue(target, targetList, null);
                        }
                    }
                }
            }
            else
            {
                var mapper = GetMapperOrDefault(sourceProperty.PropertyType, targetProperty.PropertyType);
                if (mapper != null)
                {
                    if (targetPropValue == null)
                    {
                        targetPropValue = Activator.CreateInstance(targetProperty.PropertyType);
                    }

                    targetPropValue = mapper.Map(sourcePropValue, targetPropValue, sourceProperty.PropertyType, targetProperty.PropertyType, propertyPath + ".", context);

                    targetProperty.SetValue(target, targetPropValue, null);
                }
            }
        }

        protected virtual void MapSimpleProperty(PropertyInfo targetProperty, PropertyInfo sourceProperty, object sourcePropValue, object source, object target, Type sourceType, Type targetType, string propertyPath, MappingContext context)
        {
            if (!targetProperty.CanWrite)
            {
                return;
            }

            var targetPropValue = sourcePropValue;
            if (targetProperty.PropertyType.IsEnum)
            {
                targetPropValue = Enum.Parse(targetProperty.PropertyType, sourcePropValue.ToString(), true);
            }

            targetProperty.SetValue(target, sourcePropValue, null);
        }

        protected IObjectMapper GetMapperOrDefault(Type sourceType, Type targetType)
        {
            return GetMapper(sourceType, targetType) ?? ObjectMapper.Default;
        }

        private object ResolveSourcePropertyValue(PropertyInfo property, object source, MappingContext context)
        {
            return SourcePropertyValueResolver.Resolve(property, source, context);
        }

        protected bool IsComplexType(Type type)
        {
            return type != typeof(String) && !type.IsValueType;
        }
    }
}
