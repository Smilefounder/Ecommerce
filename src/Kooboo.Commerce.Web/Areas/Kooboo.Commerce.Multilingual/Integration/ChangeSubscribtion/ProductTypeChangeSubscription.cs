using Kooboo.Commerce.Events;
using Kooboo.Commerce.Events.ProductTypes;
using Kooboo.Commerce.Multilingual.Storage;
using Kooboo.Commerce.Products;
using Kooboo.Commerce.Web.Framework.UI.Form;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;

namespace Kooboo.Commerce.Multilingual.Integration.ChangeSubscribtion
{
    class ProductTypeChangeSubscription : IHandle<ProductTypeUpdated>, IHandle<ProductTypeDeleted>
    {
        private IServiceFactory _serviceFactory;
        private ILanguageStore _languageStore;
        private ITranslationStore _translationStore;

        public ProductTypeChangeSubscription(IServiceFactory serviceFactory, ILanguageStore languageStore, ITranslationStore translationStore)
        {
            _serviceFactory = serviceFactory;
            _languageStore = languageStore;
            _translationStore = translationStore;
        }

        public void Handle(ProductTypeUpdated @event)
        {
            var productType = _serviceFactory.ProductTypes.GetById(@event.ProductTypeId);
            var controls = FormControls.Controls().ToList(); // Consider duplicate IsSelectionList in the CustomFieldDefinition class?

            var key = new EntityKey(typeof(ProductType), @event.ProductTypeId);
            var updates = new Dictionary<string, string>();

            AddFieldDefinitionUpdates(productType.CustomFieldDefinitions, "CustomFieldDefinitions", updates, controls);
            AddFieldDefinitionUpdates(productType.VariantFieldDefinitions, "VariantFieldDefinitions", updates, controls);

            foreach (var lang in _languageStore.All())
            {
                _translationStore.MarkOutOfDate(CultureInfo.GetCultureInfo(lang.Name), key, updates);
            }
        }

        private void AddFieldDefinitionUpdates(IEnumerable<CustomFieldDefinition> definitions, string prefix, Dictionary<string, string> updates, List<IFormControl> controls)
        {
            foreach (var definition in definitions)
            {
                var itemPrefix = prefix + "[" + definition.Name + "].";

                updates.Add(itemPrefix + "Label", definition.Label);

                var control = controls.Find(c => c.Name == definition.ControlType);
                if (control.IsSelectionList || control.IsValuesPredefined)
                {
                    if (control.IsSelectionList)
                    {
                        foreach (var item in definition.SelectionItems)
                        {
                            updates.Add(itemPrefix + "SelectionItems[" + item.Value + "]", item.Text);
                        }
                    }
                    else
                    {
                        updates.Add(prefix + "DefaultValue", definition.DefaultValue);
                    }
                }
            }
        }

        public void Handle(ProductTypeDeleted @event)
        {
            var key = new EntityKey(typeof(ProductType), @event.ProductTypeId);

            foreach (var lang in _languageStore.All())
            {
                _translationStore.Delete(CultureInfo.GetCultureInfo(lang.Name), key);
            }
        }
    }
}