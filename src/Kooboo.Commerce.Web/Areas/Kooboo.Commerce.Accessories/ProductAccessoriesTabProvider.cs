using Kooboo.CMS.Common.Runtime.Dependency;
using Kooboo.CMS.Sites.Extension.UI;
using Kooboo.CMS.Sites.Extension.UI.Tabs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Kooboo.Commerce.Accessories
{
    [Dependency(typeof(ITabProvider), Key = "ProductAccessoriesTabProvider")]
    public class ProductAccessoriesTabProvider : Kooboo.CMS.Sites.Extension.UI.Tabs.ITabProvider
    {
        public CMS.Sites.Extension.UI.MvcRoute[] ApplyTo
        {
            get
            {
                return new[] 
                {
                    new MvcRoute
                    {
                        Area = "Commerce",
                        Controller = "Product",
                        Action = "Edit"
                    }
                };
            }
        }

        public IEnumerable<CMS.Sites.Extension.UI.Tabs.TabInfo> GetTabs(System.Web.Routing.RequestContext requestContext)
        {
            yield return new TabInfo
            {
                Name = "Accessories",
                DisplayText = "Accessories",
                VirtualPath = "~/Areas/" + Strings.AreaName + "/Views/ProductAccessories.cshtml"
            };
        }
    }
}