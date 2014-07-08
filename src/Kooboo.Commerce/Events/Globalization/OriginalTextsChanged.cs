using Kooboo.Commerce.Globalization;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.Events.Globalization
{
    public class OriginalTextsChanged : Event
    {
        public IDictionary<EntityProperty, string> NewTexts { get; private set; }

        public OriginalTextsChanged(IDictionary<EntityProperty, string> newTexts)
        {
            Require.NotNull(newTexts, "texts");
            NewTexts = newTexts;
        }
    }
}
