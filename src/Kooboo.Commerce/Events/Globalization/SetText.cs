using Kooboo.Commerce.Globalization;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.Events.Globalization
{
    public class SetText : Event
    {
        public ILocalizable Entity { get; private set; }

        public IDictionary<EntityProperty, string> Texts { get; private set; }

        public CultureInfo Culture { get; private set; }

        public SetText(ILocalizable entity, IDictionary<EntityProperty, string> texts, CultureInfo culture)
        {
            Require.NotNull(texts, "texts");
            Require.NotNull(culture, "culture");

            Entity = entity;
            Texts = texts;
            Culture = culture;
        }
    }
}
