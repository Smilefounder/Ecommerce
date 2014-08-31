using Kooboo.Commerce.CMSIntegration.Plugins.Customers.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kooboo.CMS.Sites.Membership;
using Kooboo.Commerce.Api;

namespace Kooboo.Commerce.CMSIntegration.Plugins.Customers
{
    public class ChangeDefaultBillingAddressPlugin : SubmissionPluginBase<ChangeDefaultBillingAddressModel>
    {
        protected override SubmissionExecuteResult Execute(ChangeDefaultBillingAddressModel model)
        {
            var member = HttpContext.Membership().GetMembershipUser();
            var customer = Api.Customers.Query().ByAccountId(member.UUID).FirstOrDefault();

            Api.Customers.SetDefaultBillingAddress(customer.Id, model.AddressId);

            return new SubmissionExecuteResult
            {
                RedirectUrl = ResolveUrl(model.ReturnUrl, ControllerContext)
            };
        }
    }
}
