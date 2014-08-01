using Kooboo.Commerce.Events;
using Kooboo.Commerce.Globalization;
using Kooboo.Commerce.Globalization.Events;
using Kooboo.Commerce.Multilingual.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Kooboo.Commerce.Multilingual.Integration
{
    class GetTextEventSubscription : IHandle<GetText>
    {
        private ITranslationStore _translationStore;

        public GetTextEventSubscription(ITranslationStore translationStore)
        {
            _translationStore = translationStore;
        }

        public void Handle(GetText @event)
        {
            var entityKeys = @event.TextInfos.Select(m => m.Key).ToArray();
            var translations = _translationStore.Find(@event.Culture, entityKeys);

            for (var i = 0; i < translations.Length; i++)
            {
                var translation = translations[i];
                if (translation == null)
                {
                    continue;
                }

                foreach (var prop in translation.PropertyTranslations)
                {
                    if (String.IsNullOrEmpty(prop.TranslatedText))
                    {
                        continue;
                    }

                    @event.SetText(translation.EntityKey, prop.Property, prop.TranslatedText);
                }
            }
        }
    }
}