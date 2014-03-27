using Kooboo.CMS.Common.Runtime.Dependency;
using Kooboo.Commerce.API.Customers;
using Kooboo.Commerce.API.Locations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.API.RestProvider.Customers
{
    /// <summary>
    /// customer api
    /// </summary>
    [Dependency(typeof(ICustomerAPI), ComponentLifeStyle.Transient)]
    public class CustomerAPI : RestApiAccessBase<Customer>, ICustomerAPI
    {
        /// <summary>
        /// add id filter to query
        /// </summary>
        /// <param name="id">customer id</param>
        /// <returns>customer query</returns>
        public ICustomerQuery ById(int id)
        {
            QueryParameters.Add("id", id.ToString());
            return this;
        }

        /// <summary>
        /// add account id filter to query
        /// </summary>
        /// <param name="accountId">customer account id</param>
        /// <returns>customer query</returns>
        public ICustomerQuery ByAccountId(string accountId)
        {
            QueryParameters.Add("accountId", accountId);
            return this;
        }

        /// <summary>
        /// add first name filter to query
        /// </summary>
        /// <param name="firstName">customer first name</param>
        /// <returns>customer query</returns>
        public ICustomerQuery ByFirstName(string firstName)
        {
            QueryParameters.Add("firstName", firstName);
            return this;
        }

        /// <summary>
        /// add middle name filter to query
        /// </summary>
        /// <param name="middleName">customer middle name</param>
        /// <returns>customer query</returns>
        public ICustomerQuery ByMiddleName(string middleName)
        {
            QueryParameters.Add("middleName", middleName);
            return this;
        }

        /// <summary>
        /// add last name filter to query
        /// </summary>
        /// <param name="lastName">customer last name</param>
        /// <returns>customer query</returns>
        public ICustomerQuery ByLastName(string lastName)
        {
            QueryParameters.Add("lastName", lastName);
            return this;
        }

        /// <summary>
        /// add email filter to query
        /// </summary>
        /// <param name="email">customer email</param>
        /// <returns>customer query</returns>
        public ICustomerQuery ByEmail(string email)
        {
            QueryParameters.Add("email", email);
            return this;
        }

        /// <summary>
        /// add gender filter to query
        /// </summary>
        /// <param name="gender">customer gender</param>
        /// <returns>customer query</returns>
        public ICustomerQuery ByGender(Gender gender)
        {
            QueryParameters.Add("gender", ((int)gender).ToString());
            return this;
        }

        /// <summary>
        /// add phone filter to query
        /// </summary>
        /// <param name="phone">customer phone</param>
        /// <returns>customer query</returns>
        public ICustomerQuery ByPhone(string phone)
        {
            QueryParameters.Add("phone", phone);
            return this;
        }

        /// <summary>
        /// add city filter to query
        /// </summary>
        /// <param name="city">customer city</param>
        /// <returns>customer query</returns>
        public ICustomerQuery ByCity(string city)
        {
            QueryParameters.Add("city", city);
            return this;
        }

        /// <summary>
        /// add country id filter to query
        /// </summary>
        /// <param name="countryId">customer country id</param>
        /// <returns>customer query</returns>
        public ICustomerQuery ByCountry(int countryId)
        {
            QueryParameters.Add("countryId", countryId.ToString());
            return this;
        }

        /// <summary>
        /// load the customer/customers with country
        /// </summary>
        /// <returns>customer query</returns>
        public ICustomerQuery LoadWithCountry()
        {
            QueryParameters.Add("LoadWithCountry", "true");
            return this;
        }

        /// <summary>
        /// load the customer/customers with addresses
        /// </summary>
        /// <returns>customer query</returns>
        public ICustomerQuery LoadWithAddresses()
        {
            QueryParameters.Add("LoadWithAddresses", "true");
            return this;
        }

        /// <summary>
        /// load the customer/customers with loyalty
        /// </summary>
        /// <returns>customer query</returns>
        public ICustomerQuery LoadWithCustomerLoyalty()
        {
            QueryParameters.Add("LoadWithCustomerLoyalty", "true");
            return this;
        }

        /// <summary>
        /// create customer query
        /// </summary>
        /// <returns>customer query</returns>
        public ICustomerQuery Query()
        {
            return this;
        }

        /// <summary>
        /// create customer data access
        /// </summary>
        /// <returns>customer data access</returns>
        public ICustomerAccess Access()
        {
            return this;
        }
    }
}
