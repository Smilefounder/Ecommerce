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

            var instance = CommerceInstance.Current;
            var database = instance.Database;

            foreach (var lang in LanguageStores.Get(instance.Name).All())
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
            var store = TranslationStores.Get(CommerceInstance.Current.Name);

            var totalPending = originalDataQuery.Count() - store.TotalTranslated(culture, typeof(T))
                                                         + store.TotalOutOfDate(culture, typeof(T));

            parent.Items.Add(new LanguageSpecificMenuItem(culture.Name)
            {
                Text = text,
                Controller = controller,
                Badge = new Badge
                {
                    Text = totalPending.ToString(),
                    HtmlAttributes = new Dictionary<string, object>
                    {
                        { "class", "badge badge-danger" }
                    }
                }
            });
        }
    }
}