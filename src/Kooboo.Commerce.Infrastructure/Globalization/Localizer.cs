using Kooboo.Commerce.Data;
using Kooboo.Commerce.Events;
using Kooboo.Commerce.Globalization;
using Kooboo.Commerce.Globalization.Events;
using Kooboo.Commerce.Reflection;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;

namespace Kooboo.Commerce
{
    public static class Localizer
    {
        public static string GetText(this ILocalizable entity, string property, CultureInfo culture)
        {
            var texts = GetText(entity, new[] { property }, culture);
            return texts[property];
        }

        public static TextDictionary GetText(this ILocalizable entity, IEnumerable<string> properties, CultureInfo culture)
        {
            var textInfo = GetTextInfo(entity, properties);
            var result = GetTextInfos(new Dictionary<EntityKey, EntityTextInfo> { { textInfo.EntityKey, textInfo } }, culture);
            return result[textInfo.EntityKey].Properties;
        }

        static EntityTextInfo GetTextInfo(ILocalizable entity, IEnumerable<string> properties)
        {
            var entityKey = EntityKey.Get(entity);
            var entityProperties = new Dictionary<string, string>();

            foreach (var propName in properties)
            {
                var prop = TypeHelper.GetProperty(entityKey.EntityType, propName);
                if (prop != null)
                {
                    var value = prop.GetValue(entity, null) as string;
                    entityProperties.Add(propName, value);
                }
            }

            return new EntityTextInfo(entityKey, entityProperties);
        }

        static IDictionary<EntityKey, EntityTextInfo> GetTextInfos(IDictionary<EntityKey, EntityTextInfo> originalTextInfos, CultureInfo culture)
        {
            var @event = new GetText(originalTextInfos, culture);
            Event.Raise(@event);
            return @event.TextInfos;
        }
    }
}
