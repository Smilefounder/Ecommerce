using Kooboo.CMS.Common.Runtime.Dependency;
using Kooboo.Commerce.Web.Framework.UI.Menu;
using Kooboo.Web.Mvc.Menu;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Kooboo.Commerce.Multilingual
{
    public class MultilingualMenuInjection : CommerceInstanceMenuInjection
    {
        public override void Inject(Menu menu, System.Web.Mvc.ControllerContext controllerContext)
        {
            var root = new MenuItem
            {
                Name = "MultiLanguage",
                Text = "Multi-language"
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
        }
    }
}