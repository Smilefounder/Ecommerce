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
            var originalPropertyTexts = new Dictionary<EntityProperty, string>();
            foreach (var entity in entities)
            {
                foreach (var each in GetEntityPropertiesWithOriginalTexts<T>(entity, properties))
                {
                    originalPropertyTexts.Add(each.Key, each.Value);
                }
            }

            var propertyTexts = GetTexts(originalPropertyTexts, culture);
            var targetsByKey = new Dictionary<EntityKey, object>();

            foreach (var target in targets)
            {
                var targetKey = EntityKey.Get<TTarget>(target);
                // Change key type to orignal entity type, so we can compare them later
                targetKey = new EntityKey(typeof(T), targetKey.Values);
                targetsByKey.Add(targetKey, target);
            }

            foreach (var each in propertyTexts)
            {
                var target = targetsByKey[each.Key.EntityKey];
                var prop = TypeHelper.GetProperty(target.GetType(), each.Key.Name);
                if (prop != null)
                {
                    prop.SetValue(target, each.Value, null);
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
            var result = GetTexts(GetEntityPropertiesWithOriginalTexts<T>(entity, properties), culture);
            return result.ToDictionary(x => x.Key.Name, x => x.Value);
        }

        static IDictionary<EntityProperty, string> GetEntityPropertiesWithOriginalTexts<T>(T entity, IEnumerable<string> properties)
            where T : class, ILocalizable
        {
            var entityKey = EntityKey.Get<T>(entity);
            var entityProperties = new Dictionary<EntityProperty, string>();

            foreach (var propName in properties)
            {
                var prop = TypeHelper.GetProperty(entityKey.EntityType, propName);
                if (prop != null)
                {
                    var value = prop.GetValue(entity, null) as string;
                    entityProperties.Add(new EntityProperty(propName, entityKey), value);
                }
            }

            return entityProperties;
        }

        static IDictionary<EntityProperty, string> GetTexts(IDictionary<EntityProperty, string> originalTexts, CultureInfo culture)
        {
            var @event = new GetTexts(originalTexts, culture);
            Event.Raise(@event);
            return @event.Texts;
        }

        public static void NotifyOriginalTextChanged<T>(this T entity, string property)
            where T : class, ILocalizable
        {
            NotifyOriginalTextsChanged(entity, new[] { property });
        }

        public static void NotifyOriginalTextsChanged<T>(this T entity, IEnumerable<string> properties)
            where T : class, ILocalizable
        {
            var entityKey = EntityKey.Get<T>(entity);
            var texts = new Dictionary<EntityProperty, string>();
            foreach (var propName in properties)
            {
                var prop = TypeHelper.GetProperty(entityKey.EntityType, propName);
                if (prop != null)
                {
                    var propValue = prop.GetValue(entity, null) as string;
                    texts.Add(new EntityProperty(propName, entityKey), propValue);
                }
            }

            Event.Raise(new OriginalTextsChanged(texts));
        }
    }
}
