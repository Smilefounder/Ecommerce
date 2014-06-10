using Kooboo.Commerce.CMSIntegration.Plugins.Customers.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Kooboo.CMS.Sites.Membership;
using Kooboo.Commerce.API.Locations;

namespace Kooboo.Commerce.CMSIntegration.Plugins.Customers
{
    public class AddAddressPlugin : SubmissionPluginBase<AddAddressModel>
    {
        protected override SubmissionExecuteResult Execute(AddAddressModel model)
        {
            var member = HttpContext.Membership().GetMembershipUser();
            var customer = Site.Commerce().Customers.ByAccountId(member.UUID).FirstOrDefault();

            var address = new Address
            {
                FirstName = model.FirstName,
                LastName = model.LastName,
                Postcode = model.Postcode,
                Phone = model.Phone,
                City = model.City,
                CountryId = model.CountryId,
                Address1 = model.Address1,
                Address2 = model.Address2
            };

            if (String.IsNullOrEmpty(address.FirstName) && String.IsNullOrEmpty(address.LastName))
            {
                address.FirstName = customer.FirstName;
                address.LastName = customer.LastName;
            }

            Site.Commerce().Customers.AddAddress(customer.Id, address);

            return new SubmissionExecuteResult
            {
                Data = new AddAddressResult
                {
                    AddressId = address.Id
                }
            };
        }
    }
}
