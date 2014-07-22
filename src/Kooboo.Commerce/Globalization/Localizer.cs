using Kooboo.Commerce.Data;
using Kooboo.Commerce.Events;
using Kooboo.Commerce.Events.Globalization;
using Kooboo.Commerce.Globalization;
using Kooboo.Commerce.Utils;
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
        public static void CollectionLocalize<T, TTarget>(this IEnumerable<T> entities, IEnumerable<TTarget> targets, IEnumerable<string> properties, CultureInfo culture)
            where T : class, ILocalizable
            where TTarget: class
        {
            IDictionary<EntityKey, EntityTextInfo> textInfos = new Dictionary<EntityKey, EntityTextInfo>();
            foreach (var entity in entities)
            {
                var textInfo = GetTextInfo<T>(entity, properties);
                textInfos.Add(textInfo.EntityKey, textInfo);
            }

            textInfos = GetTextInfos(textInfos, culture);
            var targetsByKey = new Dictionary<EntityKey, object>();

            foreach (var target in targets)
            {
                var targetKey = EntityKey.Get<TTarget>(target);
                // Change key type to orignal entity type, so we can compare them later
                targetKey = new EntityKey(typeof(T), targetKey.Value);
                targetsByKey.Add(targetKey, target);
            }

            foreach (var each in textInfos)
            {
                var target = targetsByKey[each.Key];
                foreach (var propText in each.Value.PropertyTexts)
                {
                    var prop = TypeHelper.GetProperty(target.GetType(), propText.Key);
                    prop.SetValue(target, propText.Value, null);
                }
            }
        }

        public static void Localize<T, TTarget>(this T entity, TTarget target, IEnumerable<string> properties, CultureInfo culture)
            where T : class, ILocalizable
            where TTarget : class
        {
            var targetType = typeof(TTarget);
            var texts = entity.GetTexts<T>(properties, culture);
            foreach (var each in texts)
            {
                var prop = TypeHelper.GetProperty(targetType, each.Key);
                if (prop != null)
                {
                    prop.SetValue(target, each.Value, null);
                }
            }
        }

        public static string GetText<T>(this T entity, string property, CultureInfo culture)
            where T : class, ILocalizable
        {
            var texts = GetTexts<T>(entity, new[] { property }, culture);
            return texts[property];
        }

        public static IDictionary<string, string> GetTexts<T>(this T entity, IEnumerable<string> properties, CultureInfo culture)
            where T : class, ILocalizable
        {
            var textInfo = GetTextInfo<T>(entity, properties);
            var result = GetTextInfos(new Dictionary<EntityKey, EntityTextInfo> { { textInfo.EntityKey, textInfo } }, culture);
            return result[textInfo.EntityKey].PropertyTexts;
        }

        static EntityTextInfo GetTextInfo<T>(T entity, IEnumerable<string> properties)
            where T : class, ILocalizable
        {
            var entityKey = EntityKey.Get<T>(entity);
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
