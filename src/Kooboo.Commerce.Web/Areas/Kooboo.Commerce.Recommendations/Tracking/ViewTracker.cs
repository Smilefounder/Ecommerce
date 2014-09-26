using Kooboo.CMS.Sites.Extension;
using Kooboo.CMS.Sites.Models;
using Kooboo.Commerce.Recommendations.Engine.Behaviors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Kooboo.Commerce.CMSIntegration;
using Kooboo.CMS.Common.Runtime.Dependency;
using Kooboo.CMS.Sites.DataRule;

namespace Kooboo.Commerce.Recommendations.CMSIntegration
{
    [Dependency(typeof(IPageRequestModule), ComponentLifeStyle.Singleton, Key = "Kooboo.Commerce.Recommendations.ViewTracker")]
    public class ViewTracker : IPageRequestModule
    {
        public void OnResolvedPage(System.Web.Mvc.ControllerContext controllerContext, CMS.Sites.View.PageRequestContext pageRequestContext)
        {
            var page = pageRequestContext.Page;
            if (!page.CustomFields.ContainsKey("ProductId"))
            {
                return;
            }

            var value = ParameterizedFieldValue.GetFieldValue(page.CustomFields["ProductId"], pageRequestContext.GetValueProvider());
            if (String.IsNullOrEmpty(value))
            {
                return;
            }

            int productId;
            if (Int32.TryParse(value, out productId))
            {
                BehaviorReceivers.Receive(Site.Current.CommerceInstanceName(), 
                            new Behavior
                            {
                                Type = BehaviorTypes.View,
                                ItemId = productId.ToString(),
                                UserId = controllerContext.RequestContext.HttpContext.EnsureVisitorUniqueId()
                            });
            }
        }

        public void OnResolvedSite(HttpContextBase httpContext)
        {
        }

        public void OnResolvingPage(System.Web.Mvc.ControllerContext controllerContext)
        {
        }

        public void OnResolvingSite(HttpContextBase httpContext)
        {
        }
    }
}