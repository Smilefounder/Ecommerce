using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kooboo.CMS.Sites.Membership;
using Kooboo.Commerce.CMSIntegration.Plugins.Customers.Models;

namespace Kooboo.Commerce.CMSIntegration.Plugins.Customers
{
    public class LogoutCustomerPlugin : SubmissionPluginBase<LogoutCustomerModel>
    {
        protected override SubmissionExecuteResult Execute(LogoutCustomerModel model)
        {
            HttpContext.Membership().SignOut();
            return new SubmissionExecuteResult
            {
                RedirectUrl = ResolveUrl(model.ReturnUrl, ControllerContext)
            };
        }
    }
}
