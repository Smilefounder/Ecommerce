using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Collections;

namespace Kooboo.Commerce.API.HAL
{
    public class DefaultPropertyResourceProvider : IPropertyResourceProvider
    {
        public string Name
        {
            get { return "Default Property Link Provider"; }
        }

        public string Description
        {
            get { return "Automatically add relation links to the resouce according to the reference properties of entity type."; }
        }

        public IEnumerable<PropertyResource> GetPropertyResources(ResourceDescriptor descriptor, object entity)
        {
            var propResources = new List<PropertyResource>();
            Type type = entity.GetType();
            var properties = type.GetProperties(BindingFlags.Public | BindingFlags.Instance);
            foreach(var prop in properties)
            {
                var isEnumerable = false;
                Type propType = prop.PropertyType;
                if (prop.PropertyType.IsGenericType)
                {
                    var defType = prop.PropertyType.GetGenericTypeDefinition();
                    if (typeof(IEnumerable).IsAssignableFrom(defType))
                    {
                        isEnumerable = true;
                    }
                    propType = prop.PropertyType.GetGenericArguments()[0];
                }
                if (prop.PropertyType.IsArray)
                {
                    propType = prop.PropertyType.GetElementType();
                    isEnumerable = true;
                }
                if (typeof(IItemResource).IsAssignableFrom(propType))
                {
                    var resourcePrefix = propType.Name;
                    var propVal = prop.GetValue(entity, null);
                    if (propVal != null)
                    {
                        var propResource = new PropertyResource();
                        propResource.IsEnumerable = isEnumerable;
                        propResource.Value = propVal;
                        propResource.ResourceNames.Add(ResourceDescriptor.NomalizeResourceName(resourcePrefix, "detail"));
                        propResources.Add(propResource);
                    }
                }
            }
            return propResources;
        }
    }
}
