using Kooboo.Commerce.Globalization;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.Events.Globalization
{
    public class GetTexts : Event
    {
        public IDictionary<EntityProperty, string> Texts { get; private set; }

        public CultureInfo Culture { get; private set; }

        public GetTexts(IDictionary<EntityProperty, string> texts, CultureInfo culture)
        {
            Culture = culture;
            Texts = texts;
        }
    }
}
