using Kooboo.CMS.Common.Runtime.Dependency;
using Kooboo.CMS.Sites.Extension.UI;
using Kooboo.CMS.Sites.Extension.UI.Tabs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Kooboo.Commerce.Recommendations
{
    [Dependency(typeof(ITabProvider), Key = "ProductRecommendationsTabProvider")]
    public class ProductRecommendationsTabProvider : ITabProvider
    {
        public CMS.Sites.Extension.UI.MvcRoute[] ApplyTo
        {
            get
            {
                return new MvcRoute[] {
                    new MvcRoute {
                        Area = "Commerce",
                        Controller = "Product",
                        Action = "Edit"
                    }
                };
            }
        }

        public IEnumerable<TabInfo> GetTabs(System.Web.Routing.RequestContext requestContext)
        {
            yield return new TabInfo
            {
                Name = "Recommendations",
                DisplayText = "Recommendations",
                VirtualPath = "~/Areas/" + Strings.AreaName + "/Views/Recommendations.cshtml"
            };
        }
    }
}