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
    public class ChangeDefaultShippingAddressPlugin : SubmissionPluginBase<ChangeDefaultShippingAddressModel>
    {
        protected override SubmissionExecuteResult Execute(ChangeDefaultShippingAddressModel model)
        {
            var member = HttpContext.Membership().GetMembershipUser();
            var customer = Api.Customers.Query().ByEmail(member.Email).FirstOrDefault();

            Api.Customers.SetDefaultShippingAddress(customer.Id, model.AddressId);

            return new SubmissionExecuteResult
            {
                RedirectUrl = ResolveUrl(model.ReturnUrl, ControllerContext)
            };
        }
    }
}
