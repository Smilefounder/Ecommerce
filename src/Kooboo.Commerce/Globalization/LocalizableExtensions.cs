using Kooboo.Commerce.Data;
using Kooboo.Commerce.Events;
using Kooboo.Commerce.Events.Globalization;
using Kooboo.Commerce.Globalization;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce
{
    public static class LocalizableExtensions
    {
        public static string GetText(this ILocalizable entity, string property, CultureInfo culture)
        {
            var texts = GetTexts(entity, new[] { property }, culture);
            return texts[property];
        }

        public static IDictionary<string, string> GetTexts(this ILocalizable entity, IEnumerable<string> properties, CultureInfo culture)
        {
            var entityKey = EntityKey.GetEntityKey(entity);
            var entityProperties = properties.Select(name => new EntityProperty(name, entityKey));

            var result = GetTexts(entity, entityProperties, culture);
            return result.ToDictionary(x => x.Key.Name, x => x.Value);
        }

        static IDictionary<EntityProperty, string> GetTexts(this ILocalizable entity, IEnumerable<EntityProperty> properties, CultureInfo culture)
        {
            var @event = new GetText(entity, properties, culture);
            Event.Raise(@event);
            return @event.Texts;
        }

        public static void SetText(this ILocalizable entity, string property, string propertyText, CultureInfo culture)
        {
            SetTexts(entity, new Dictionary<string, string> { { property, propertyText } }, culture);
        }

        public static void SetTexts(this ILocalizable entity, IDictionary<string, string> propertyTexts, CultureInfo culture)
        {
            var entityKey = EntityKey.GetEntityKey(entity);
            var texts = propertyTexts.ToDictionary(x => new EntityProperty(x.Key, entityKey), x => x.Value);

            var @event = new SetText(entity, texts, culture);
            Event.Raise(@event);
        }
    }
}
