using Kooboo.CMS.Common.Runtime;
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
    public class RegisterCustomerPlugin : SubmissionPluginBase<RegisterCustomerModel>
    {
        private MembershipUserManager _userManager;

        public RegisterCustomerPlugin(MembershipUserManager userManager)
        {
            _userManager = userManager;

            Parameters["SetAuthCookie"] = true;
        }

        protected override SubmissionExecuteResult Execute(RegisterCustomerModel model)
        {
            var membership = MemberPluginHelper.GetMembership();
            var user = _userManager.Create(membership, model.Email, model.Email, model.Password, true, "en-US", null);

            var customer = new Customer
            {
                Email = model.Email,
                FirstName = model.FirstName,
                LastName = model.LastName,
                Gender = model.Gender
            };

            if (model.CustomFields != null)
            {
                foreach (var each in model.CustomFields)
                {
                    customer.CustomFields.Add(each.Key, each.Value);
                }
            }

            var customerId = Site.Commerce().Customers.Create(customer);

            if (model.SetAuthCookie)
            {
                var auth = new MembershipAuthentication(Site, membership, HttpContext);
                auth.SetAuthCookie(customer.Email, false);

                var sessionId = EngineContext.Current.Resolve<IShoppingCartSessionIdProvider>().GetCurrentSessionId(false);
                if (!String.IsNullOrWhiteSpace(sessionId))
                {
                    Site.Commerce().ShoppingCarts.MigrateCart(customerId, sessionId);
                }
            }

            return new SubmissionExecuteResult
            {
                RedirectUrl = ResolveUrl(model.ReturnUrl, ControllerContext),
                Data = new RegisterCustomerResult
                {
                    CustomerId = customer.Id
                }
            };
        }
    }
}
