using Kooboo.Commerce.Globalization;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.Events.Globalization
{
    public class GetText : Event
    {
        public ILocalizable Entity { get; private set; }

        public IDictionary<EntityProperty, string> Texts { get; private set; }

        public CultureInfo Culture { get; private set; }

        public GetText(ILocalizable entity, IEnumerable<EntityProperty> properties, CultureInfo culture)
        {
            Entity = entity;
            Culture = culture;

            Texts = new Dictionary<EntityProperty, string>();

            foreach (var prop in properties)
            {
                if (!Texts.ContainsKey(prop))
                {
                    Texts.Add(prop, null);
                }
            }
        }
    }
}
