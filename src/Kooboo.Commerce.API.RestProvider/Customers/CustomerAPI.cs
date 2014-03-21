using Kooboo.CMS.Common.Runtime.Dependency;
using Kooboo.Commerce.API.Customers;
using Kooboo.Commerce.API.Locations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.API.RestProvider.Customers
{
    [Dependency(typeof(ICustomerAPI), ComponentLifeStyle.Transient)]
    public class CustomerAPI : RestApiAccessBase<Customer>, ICustomerAPI
    {
        public ICustomerQuery ById(int id)
        {
            QueryParameters.Add("id", id.ToString());
            return this;
        }

        public ICustomerQuery ByAccountId(string accountId)
        {
            QueryParameters.Add("accountId", accountId);
            return this;
        }

        public ICustomerQuery ByFirstName(string firstName)
        {
            QueryParameters.Add("firstName", firstName);
            return this;
        }

        public ICustomerQuery ByMiddleName(string middleName)
        {
            QueryParameters.Add("middleName", middleName);
            return this;
        }

        public ICustomerQuery ByLastName(string lastName)
        {
            QueryParameters.Add("lastName", lastName);
            return this;
        }

        public ICustomerQuery ByEmail(string email)
        {
            QueryParameters.Add("email", email);
            return this;
        }

        public ICustomerQuery ByGender(Gender gender)
        {
            QueryParameters.Add("gender", ((int)gender).ToString());
            return this;
        }

        public ICustomerQuery ByPhone(string phone)
        {
            QueryParameters.Add("phone", phone);
            return this;
        }

        public ICustomerQuery ByCity(string city)
        {
            QueryParameters.Add("city", city);
            return this;
        }

        public ICustomerQuery ByCountry(int countryId)
        {
            QueryParameters.Add("countryId", countryId.ToString());
            return this;
        }

        public ICustomerQuery LoadWithCountry()
        {
            QueryParameters.Add("LoadWithCountry", "true");
            return this;
        }

        public ICustomerQuery LoadWithAddresses()
        {
            QueryParameters.Add("LoadWithAddresses", "true");
            return this;
        }

        public ICustomerQuery LoadWithCustomerLoyalty()
        {
            QueryParameters.Add("LoadWithCustomerLoyalty", "true");
            return this;
        }

        public ICustomerQuery Query()
        {
            return this;
        }

        public ICustomerAccess Access()
        {
            return this;
        }
    }
}
