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
            var key = EntityKey.Get(entity);
            var texts = GetTextInfo(entity, properties);
            return GetText(new Dictionary<EntityKey, TextDictionary> { { key, texts } }, culture)[key];
        }

        public static string GetText(EntityKey key, string property, CultureInfo culture)
        {
            return GetText(key, new[] { property }, culture)[property];
        }

        public static IDictionary<EntityKey, string> GetText(IEnumerable<EntityKey> keys, string property, CultureInfo culture)
        {
            return GetText(keys, new[] { property }, culture).ToDictionary(it => it.Key, it => it.Value[property]);
        }

        public static IDictionary<EntityKey, TextDictionary> GetText(IEnumerable<EntityKey> keys, IEnumerable<string> properties, CultureInfo culture)
        {
            return GetText(keys.ToDictionary(it => it, _ => new TextDictionary()), culture);
        }

        public static TextDictionary GetText(EntityKey key, IEnumerable<string> properties, CultureInfo culture)
        {
            return GetText(new Dictionary<EntityKey, TextDictionary> { { key, new TextDictionary() } }, culture)[key];
        }

        static TextDictionary GetTextInfo(ILocalizable entity, IEnumerable<string> properties)
        {
            var entityKey = EntityKey.Get(entity);
            var texts = new TextDictionary();

            foreach (var propName in properties)
            {
                var prop = TypeHelper.GetProperty(entityKey.EntityType, propName);
                if (prop != null)
                {
                    var value = prop.GetValue(entity, null) as string;
                    texts.Add(propName, value);
                }
            }

            return texts;
        }

        static IDictionary<EntityKey, TextDictionary> GetText(IDictionary<EntityKey, TextDictionary> originalTextInfos, CultureInfo culture)
        {
            var @event = new GetText(originalTextInfos, culture);
            Event.Raise(@event);
            return @event.Texts;
        }
    }
}
