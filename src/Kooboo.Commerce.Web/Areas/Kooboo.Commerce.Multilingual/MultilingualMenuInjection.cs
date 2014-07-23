using Kooboo.CMS.Common.Runtime.Dependency;
using Kooboo.Commerce.Multilingual.Domain;
using Kooboo.Commerce.Multilingual.Storage;
using Kooboo.Commerce.Web.Framework.UI.Menu;
using Kooboo.Web.Mvc.Menu;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Routing;

namespace Kooboo.Commerce.Multilingual
{
    public class MultilingualMenuInjection : CommerceInstanceMenuInjection
    {
        private ILanguageStore _languageStore;

        public MultilingualMenuInjection(ILanguageStore languageStore)
        {
            _languageStore = languageStore;
        }

        public override void Inject(Menu menu, System.Web.Mvc.ControllerContext controllerContext)
        {
            var root = new MenuItem
            {
                Name = "Multiligual",
                Text = "Multiligual"
            };

            menu.Items.Add(root);

            root.Items.Add(new MenuItem
            {
                Name = "Languages",
                Text = "Languages",
                Controller = "Language",
                Action = "Index",
                Area = Strings.AreaName
            });

            foreach (var lang in _languageStore.All())
            {
                var langItem = new MenuItem
                {
                    Name = "Language-" + lang.Name,
                    Text = lang.DisplayName + " (" + lang.Name + ")"
                };

                root.Items.Add(langItem);
                AddLanguageChildItems(langItem, lang);
            }
        }

        private void AddLanguageChildItems(MenuItem langItem, Language lang)
        {
            var routeValues = new RouteValueDictionary();
            routeValues.Add("culture", lang.Name);

            langItem.Items.Add(new MenuItem
            {
                Text = "Brands",
                Controller = "Brand",
                Action = "Index",
                Area = Strings.AreaName,
                RouteValues = routeValues
            });
            langItem.Items.Add(new MenuItem
            {
                Text = "Categories",
                Controller = "Category",
                Action = "Index",
                Area = Strings.AreaName,
                RouteValues = routeValues
            });
            langItem.Items.Add(new MenuItem
            {
                Text = "Products",
                Controller = "Product",
                Action = "Index",
                Area = Strings.AreaName,
                RouteValues = routeValues
            });
        }
    }
}