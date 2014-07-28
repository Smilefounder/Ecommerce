using Kooboo.Commerce.Globalization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Kooboo.Commerce.Api.Local.Mapping
{
    public interface IPropertyValueResolver
    {
        object Resolve(PropertyInfo property, object container, MappingContext context);
    }

    public class DefaultPropertyValueResolver : IPropertyValueResolver
    {
        public virtual object Resolve(PropertyInfo property, object container, MappingContext context)
        {
            return property.GetValue(container, null);
        }
    }

    public class LocalizablePropertyValueResolver : DefaultPropertyValueResolver
    {
        public override object Resolve(PropertyInfo property, object container, MappingContext context)
        {
            var value = base.Resolve(property, container, context);

            if (property.PropertyType != typeof(String))
            {
                return value;
            }

            var localizable = container as ILocalizable;

            if (localizable != null && property.IsDefined(typeof(LocalizableAttribute), false))
            {
                var localizedValue = localizable.GetText(property.Name, context.ApiContext.Culture);
                if (localizedValue != null)
                {
                    return localizedValue;
                }
            }

            return value;
        }
    }
}
