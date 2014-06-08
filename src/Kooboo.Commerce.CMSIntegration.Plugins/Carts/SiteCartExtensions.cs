using Kooboo.CMS.Sites.Models;
using Kooboo.Commerce.API.ShoppingCarts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using Kooboo.CMS.Sites.Membership;

namespace Kooboo.Commerce.CMSIntegration.Plugins.Carts
{
    static class SiteCartExtensions
    {
        public static ShoppingCart GetCurrentCart(this Site site, ControllerContext controllerContext)
        {
            var member = controllerContext.HttpContext.Membership().GetMembershipUser();
            if (member != null)
            {
                return site.Commerce().ShoppingCarts.ByAccountId(member.UUID).FirstOrDefault();
            }

            return site.Commerce().ShoppingCarts.BySessionId(controllerContext.HttpContext.Session.SessionID).FirstOrDefault();
        }
    }
}
