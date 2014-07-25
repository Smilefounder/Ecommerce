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

            var sourcePropValue = ResolveSourcePropertyValue(sourceProperty, source);

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
                if (!context.Includes.Includes(propertyPath))
                {
                    return;
                }

                object targetPropValue = targetProperty.GetValue(target, null);

                if (context.VisitedObjects.Contains(targetPropValue))
                {
                    return;
                }

                Type targetElementType;

                if (IsCollection(targetProperty.PropertyType, out targetElementType))
                {
                    var sourceElementType = GetCollectionElementType(sourceProperty.PropertyType);

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
                        addMethod = targetList.GetType().GetMethod("Add", BindingFlags.Public | BindingFlags.Instance, null, new[] { targetElementType }, null);
                    }

                    var sourceList = sourcePropValue as IEnumerable;
                    var elementPrefix = propertyPath + ".";
                    var totalElements = 0;

                    foreach (var sourceElement in sourceList)
                    {
                        var targetElement = elementMapper.Map(sourceElement, Activator.CreateInstance(targetElementType), sourceElementType, targetElementType, elementPrefix, context);
                        addMethod.Invoke(targetList, new[] { targetElement });
                        totalElements++;
                    }

                    if (!Object.ReferenceEquals(targetList, targetPropValue))
                    {
                        if (targetProperty.PropertyType.IsArray)
                        {
                            var array = Array.CreateInstance(targetElementType, totalElements);
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
                else
                {
                    var mapper = GetMapper(sourceProperty.PropertyType, targetProperty.PropertyType);
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
            else
            {
                var targetPropValue = sourcePropValue;
                if (targetProperty.PropertyType.IsEnum)
                {
                    targetPropValue = Enum.Parse(targetProperty.PropertyType, sourcePropValue.ToString(), true);
                }

                targetProperty.SetValue(target, sourcePropValue, null);
            }
        }

        protected virtual object ResolveSourcePropertyValue(PropertyInfo property, object source)
        {
            return property.GetValue(source, null);
        }

        protected bool IsCollection(Type type, out Type elementType)
        {
            if (type.IsArray)
            {
                elementType = type.GetElementType();
                return true;
            }

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

        private Type GetCollectionElementType(Type type)
        {
            if (type.IsArray)
            {
                return type.GetElementType();
            }

            return type.GetGenericArguments()[0];
        }

        protected bool IsComplexType(Type type)
        {
            return type != typeof(String) && !type.IsValueType;
        }
    }
}
