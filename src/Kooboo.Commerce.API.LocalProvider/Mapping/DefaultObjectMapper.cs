using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Kooboo.Commerce.API.LocalProvider.Mapping
{
    public class DefaultObjectMapper : IObjectMapper
    {
        public Func<Type, Type, IObjectMapper> GetMapper = (sourceType, targetType) => ObjectMapper.GetMapper(sourceType, targetType);

        public object Map(object source, object target, Type sourceType, Type targetType, MappingContext context)
        {
            foreach (var targetProp in targetType.GetProperties(BindingFlags.Public | BindingFlags.Instance))
            {
                var prop = new ObjectProperty(target, targetProp);
                if (!context.VisitedTargetProperties.Contains(prop))
                {
                    context.VisitedTargetProperties.Add(prop);
                    MapProperty(prop, source, sourceType, targetType, context);
                }
            }

            return target;
        }

        protected virtual void MapProperty(ObjectProperty targetProperty, object source, Type sourceType, Type targetType, MappingContext context)
        {
            var sourceProperty = sourceType.GetProperty(targetProperty.PropertyName, BindingFlags.Public | BindingFlags.Instance);
            if (sourceProperty == null)
            {
                return;
            }

            var sourcePropValue = GetSourcePropertyValue(sourceProperty, source);

            if (IsComplexType(targetProperty.PropertyType))
            {
                Type targetElementType;
                object targetPropValue = targetProperty.GetValue();

                if (IsCollection(targetProperty.PropertyType, out targetElementType))
                {
                    var sourceElementType = sourceProperty.PropertyType.GetGenericArguments()[0];

                    var elementMapper = GetMapper(sourceElementType, targetElementType);
                    if (elementMapper == null)
                    {
                        return;
                    }

                    var targetList = targetPropValue;

                    var addMethod = targetProperty.PropertyType.GetMethod("Add", BindingFlags.Public | BindingFlags.Instance, null, new[] { targetElementType }, null);
                    if (targetList == null || addMethod == null)
                    {
                        targetList = Activator.CreateInstance(typeof(List<>).MakeGenericType(targetElementType));
                    }

                    var sourceList = sourcePropValue as IEnumerable;

                    foreach (var sourceElement in sourceList)
                    {
                        var targetElement = elementMapper.Map(sourceElement, Activator.CreateInstance(targetElementType), sourceElementType, targetElementType, context);
                        addMethod.Invoke(targetList, new[] { targetElement });
                    }

                    if (!Object.ReferenceEquals(targetList, targetPropValue))
                    {
                        targetProperty.SetValue(targetList);
                    }
                }
                else
                {
                    var mapper = GetMapper(sourceProperty.PropertyType, targetProperty.PropertyType);
                    if (mapper != null)
                    {
                        if (targetPropValue == null)
                        {
                            targetPropValue = Activator.CreateInstance(targetProperty.PropertyType);
                        }

                        mapper.Map(sourcePropValue, targetPropValue, sourceProperty.PropertyType, targetProperty.PropertyType, context);
                    }
                }
            }
            else
            {
                targetProperty.SetValue(sourcePropValue);
            }
        }

        protected virtual object GetSourcePropertyValue(PropertyInfo property, object source)
        {
            return property.GetValue(source, null);
        }

        protected bool IsCollection(Type type, out Type elementType)
        {
            foreach (var @interface in type.GetInterfaces())
            {
                if (@interface.IsGenericType)
                {
                    var genericTypeDef = @interface.GetGenericTypeDefinition();
                    if (genericTypeDef == typeof(IEnumerable<>))
                    {
                        elementType = @interface.GetGenericArguments()[0];
                        return true;
                    }
                }
            }

            elementType = null;

            return false;
        }

        protected bool IsComplexType(Type type)
        {
            return type != typeof(String) && !type.IsValueType;
        }
    }
}
