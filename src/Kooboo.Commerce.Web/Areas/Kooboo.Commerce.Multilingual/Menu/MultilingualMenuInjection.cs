using Kooboo.CMS.Common.Runtime.Dependency;
using Kooboo.Commerce.Brands;
using Kooboo.Commerce.Categories;
using Kooboo.Commerce.Data;
using Kooboo.Commerce.Multilingual.Storage;
using Kooboo.Commerce.Products;
using Kooboo.Commerce.Web.Framework.UI.Menu;
using Kooboo.Web.Mvc.Menu;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Routing;

namespace Kooboo.Commerce.Multilingual
{
    public class MultilingualMenuInjection : CommerceInstanceMenuInjection
    {
        private ILanguageStore _languageStore;
        private ITranslationStore _translationStore;

        public MultilingualMenuInjection(ILanguageStore languageStore, ITranslationStore translationStore)
        {
            _languageStore = languageStore;
            _translationStore = translationStore;
        }

        public override void Inject(Menu menu, System.Web.Mvc.ControllerContext controllerContext)
        {
            if (CommerceInstance.Current == null)
            {
                return;
            }
            
            var root = new MenuItem
            {
                Name = "Multiligual",
                Text = "Multiligual"
            };

            menu.Items.Add(root);

            root.Items.Add(new MultilingualMenuItem
            {
                Name = "Languages",
                Text = "Languages",
                Controller = "Language",
                Action = "Index"
            });

            var database = CommerceInstance.Current.Database;

            foreach (var lang in _languageStore.All())
            {
                var langItem = new MenuItem
                {
                    Name = "Language-" + lang.Name,
                    Text = lang.Name
                };

                root.Items.Add(langItem);
                AddLanguageChildItems(langItem, lang, database);
            }
        }

        private void AddLanguageChildItems(MenuItem parent, Language lang, ICommerceDatabase database)
        {
            var culture = CultureInfo.GetCultureInfo(lang.Name);

            AddLanguageChildItem<Brand>(parent, culture, "Brands", "Brand", database.GetRepository<Brand>().Query());
            AddLanguageChildItem<Category>(parent, culture, "Categories", "Category", database.GetRepository<Category>().Query());
            AddLanguageChildItem<Product>(parent, culture, "Products", "Product", database.GetRepository<Product>().Query());
            AddLanguageChildItem<ProductType>(parent, culture, "Product types", "ProductType", database.GetRepository<ProductType>().Query());
        }

        private void AddLanguageChildItem<T>(MenuItem parent, CultureInfo culture, string text, string controller, IQueryable<T> originalDataQuery)
        {
            var totalPending = originalDataQuery.Count() - _translationStore.TotalTranslated(culture, typeof(T))
                                                         + _translationStore.TotalOutOfDate(culture, typeof(T));

            parent.Items.Add(new LanguageSpecificMenuItem(culture.Name)
            {
                Text = text,
                HtmlFormat = "{0} <span class='badge badge-danger'>" + totalPending + "</span>",
                Controller = controller
            });
        }
    }
}