using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kooboo.CMS.Sites.Membership;

namespace Kooboo.Commerce.CMSIntegration.Plugins.Customers
{
    public class CustomerLogoutPlugin : SubmissionPluginBase
    {
        protected override SubmissionExecuteResult Execute(SubmissionModel model)
        {
            HttpContext.Membership().SignOut();
            return null;
        }
    }
}
