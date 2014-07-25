using Kooboo.Commerce.Globalization;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Kooboo.Commerce.API.LocalProvider.Mapping
{
    public class LocalizableObjectMapper : DefaultObjectMapper
    {
        public Func<CultureInfo> GetCurrentCulture = () => CultureInfo.CurrentUICulture;

        protected override object ResolveSourcePropertyValue(PropertyInfo property, object source)
        {
            var value = base.ResolveSourcePropertyValue(property, source);

            if (property.PropertyType != typeof(String))
            {
                return value;
            }

            var container = source as ILocalizable;

            if (container != null && property.IsDefined(typeof(LocalizableAttribute), false))
            {
                var localizedValue = container.GetText(property.Name, GetCurrentCulture());
                if (localizedValue != null)
                {
                    return localizedValue;
                }
            }

            return value;
        }
    }
}
