using Kooboo.CMS.Sites.Extension;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kooboo.CMS.Sites.Membership;
using Kooboo.Commerce.CMSIntegration;
using Kooboo.Commerce.Api;
using Kooboo.CMS.Sites.Models;
using System.Web.Mvc;

namespace Kooboo.Commerce.CMSIntegration.Plugins.Customers
{
    public class RequireLoginPlugin : IPagePlugin
    {
        public System.Web.Mvc.ActionResult Execute(CMS.Sites.View.Page_Context pageContext, CMS.Sites.View.PagePositionContext positionContext)
        {
            var user = pageContext.ControllerContext.HttpContext.Membership().GetMembershipUser();
            if (user != null)
            {
                var customer = Site.Current.Commerce().Customers.Query().ByAccountId(user.UUID).FirstOrDefault();
                if (customer != null)
                {
                    return null;
                }
            }

            // TODO: How to get login url?
            var loginUrl = pageContext.FrontUrl.PageUrl("Login", new { ReturnUrl = pageContext.ControllerContext.HttpContext.Request.RawUrl }).ToString();

            return new RedirectResult(loginUrl);
        }
    }
}
