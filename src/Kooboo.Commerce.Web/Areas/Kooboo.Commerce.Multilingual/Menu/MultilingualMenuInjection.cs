using Kooboo.CMS.Common.Runtime.Dependency;
using Kooboo.Commerce.Data;
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
            langItem.Items.Add(new LanguageSpecificMenuItem(lang.Name)
            {
                Text = "Brands",
                Controller = "Brand",
                Action = "Index"
            });
            langItem.Items.Add(new LanguageSpecificMenuItem(lang.Name)
            {
                Text = "Categories",
                Controller = "Category",
                Action = "Index"
            });
            langItem.Items.Add(new LanguageSpecificMenuItem(lang.Name)
            {
                Text = "Products",
                Controller = "Product",
                Action = "Index"
            });
        }
    }
}