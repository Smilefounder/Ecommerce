using Kooboo.Commerce.CMSIntegration.Plugins.Customers.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Kooboo.CMS.Sites.Membership;
using Kooboo.Commerce.Api;
using Kooboo.Commerce.Api.Countries;
using Kooboo.Commerce.Api.Customers;

namespace Kooboo.Commerce.CMSIntegration.Plugins.Customers
{
    public class AddAddressPlugin : SubmissionPluginBase<AddAddressModel>
    {
        protected override SubmissionExecuteResult Execute(AddAddressModel model)
        {
            var member = HttpContext.Membership().GetMembershipUser();
            var customer = Site.Commerce().Customers.Query().ByEmail(member.Email).FirstOrDefault();

            var address = new Address
            {
                FirstName = model.FirstName,
                LastName = model.LastName,
                Postcode = model.Postcode,
                Phone = model.Phone,
                City = model.City,
                State = model.State,
                CountryId = model.CountryId,
                Address1 = model.Address1,
                Address2 = model.Address2
            };

            if (String.IsNullOrEmpty(address.FirstName) && String.IsNullOrEmpty(address.LastName))
            {
                address.FirstName = customer.FirstName;
                address.LastName = customer.LastName;
            }

            var addressId = Api.Customers.AddAddress(customer.Id, address);

            if (model.SetDefaultShippingAddress)
            {
                Api.Customers.SetDefaultShippingAddress(customer.Id, addressId);
            }
            if (model.SetDefaultBillingAddress)
            {
                Api.Customers.SetDefaultBillingAddress(customer.Id, addressId);
            }

            return new SubmissionExecuteResult
            {
                RedirectUrl = ResolveUrl(model.ReturnUrl, ControllerContext),
                Data = new AddAddressResult
                {
                    AddressId = addressId
                }
            };
        }
    }
}
