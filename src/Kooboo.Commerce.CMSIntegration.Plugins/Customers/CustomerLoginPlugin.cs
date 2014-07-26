using Kooboo.CMS.Membership.Services;
using Kooboo.CMS.Sites.Membership;
using Kooboo.Commerce.Api.Customers;
using Kooboo.Commerce.CMSIntegration.Plugins.Customers.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kooboo.Commerce.CMSIntegration.Plugins.Customers
{
    public class CustomerLoginPlugin : SubmissionPluginBase<CustomerLoginModel>
    {
        private MembershipUserManager _userManager;

        public CustomerLoginPlugin(MembershipUserManager userManager)
        {
            _userManager = userManager;
        }

        protected override SubmissionExecuteResult Execute(CustomerLoginModel model)
        {
            var membership = MemberPluginHelper.GetMembership();
            var valid = _userManager.Validate(membership, model.Email, model.Password);
            Customer customer = null;

            if (valid)
            {
                customer = Site.Commerce().Customers.ByEmail(model.Email).FirstOrDefault();
                if (customer == null)
                {
                    valid = false;
                }
            }

            if (!valid)
            {
                throw new InvalidModelStateException(new Dictionary<string, string>
                {
                    { "Email", "Email and/or password are incorrect." }
                });
            }

            HttpContext.Membership().SetAuthCookie(model.Email, model.RememberMe);

            var sessionId = HttpContext.CurrentSessionId();
            if (!String.IsNullOrWhiteSpace(sessionId))
            {
                Site.Commerce().ShoppingCarts.MigrateCart(customer.Id, sessionId);
            }

            var returnUrl = ResolveUrl(model.ReturnUrl, ControllerContext);

            return new SubmissionExecuteResult
            {
                RedirectUrl = returnUrl,
                Data = new CustomerLoginResult
                {
                    CustomerId = customer.Id,
                    Email = model.Email
                }
            };
        }
    }
}
