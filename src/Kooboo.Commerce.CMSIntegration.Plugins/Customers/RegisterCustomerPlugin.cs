using Kooboo.CMS.Membership.Services;
using Kooboo.CMS.Sites.Membership;
using Kooboo.Commerce.API.Customers;
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

            Parameters.Add("SetAuthCookie", true);
        }

        protected override object Execute(RegisterCustomerModel model)
        {
            var membership = MemberPluginHelper.GetMembership();
            var user = _userManager.Create(membership, model.Email, model.Email, model.Password, true, "en-US", null);

            var customer = new Customer
            {
                AccountId = user.UUID,
                Email = model.Email,
                FirstName = model.FirstName,
                MiddleName = model.MiddleName,
                LastName = model.LastName,
                Gender = model.Gender,
                Phone = model.Phone,
                City = model.City,
                CountryId = model.CountryId
            };

            if (model.CustomFields != null)
            {
                foreach (var each in model.CustomFields)
                {
                    customer.CustomFields.Add(new CustomerCustomField(each.Key, each.Value));
                }
            }

            Site.Commerce().Customers.Create(customer);

            if (model.SetAuthCookie)
            {
                var auth = new MembershipAuthentication(Site, membership, HttpContext);
                auth.SetAuthCookie(customer.Email, false);
            }

            return new RegisterCustomerResult
            {
                CustomerId = customer.Id
            };
        }
    }
}
