using Kooboo.CMS.Common.Runtime.Dependency;
using Kooboo.Commerce.Api.Local;
using Kooboo.Commerce.API.Customers;
using Kooboo.Commerce.API.Locations;
using Kooboo.Commerce.Customers.Services;
using Kooboo.Commerce.Data;
using Kooboo.Commerce.Locations.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.API.LocalProvider.Customers
{
    /// <summary>
    /// customer api
    /// </summary>
    [Dependency(typeof(ICustomerAPI), ComponentLifeStyle.Transient)]
    [Dependency(typeof(ICustomerQuery), ComponentLifeStyle.Transient)]
    public class CustomerAPI : LocalCommerceQuery<Customer, Kooboo.Commerce.Customers.Customer>, ICustomerAPI
    {
        private LocalApiContext _context;
        private ICommerceDatabase _db;
        private ICustomerService _customerService;
        private ICountryService _countryService;

        public CustomerAPI(LocalApiContext context)
        {
            _context = context;
            _db = _context.Database;
            _customerService = _context.ServiceFactory.Customers;
            _countryService = _context.ServiceFactory.Countries;
        }

        /// <summary>
        /// create entity query
        /// </summary>
        /// <returns>queryable object</returns>
        protected override IQueryable<Kooboo.Commerce.Customers.Customer> CreateQuery()
        {
            return _customerService.Query();
        }

        /// <summary>
        /// use the default order when pagination the query
        /// </summary>
        /// <param name="query">pagination query</param>
        /// <returns>ordered query</returns>
        protected override IQueryable<Kooboo.Commerce.Customers.Customer> OrderByDefault(IQueryable<Kooboo.Commerce.Customers.Customer> query)
        {
            return query.OrderBy(o => o.Id);
        }

        /// <summary>
        /// add id filter to query
        /// </summary>
        /// <param name="id">customer id</param>
        /// <returns>customer query</returns>
        public ICustomerQuery ById(int id)
        {
            Query = Query.Where(o => o.Id == id);
            return this;
        }

        /// <summary>
        /// add account id filter to query
        /// </summary>
        /// <param name="accountId">customer account id</param>
        /// <returns>customer query</returns>
        public ICustomerQuery ByAccountId(string accountId)
        {
            Query = Query.Where(o => o.AccountId == accountId);
            return this;
        }

        /// <summary>
        /// add first name filter to query
        /// </summary>
        /// <param name="firstName">customer first name</param>
        /// <returns>customer query</returns>
        public ICustomerQuery ByFirstName(string firstName)
        {
            Query = Query.Where(o => o.FirstName == firstName);
            return this;
        }

        /// <summary>
        /// add middle name filter to query
        /// </summary>
        /// <param name="middleName">customer middle name</param>
        /// <returns>customer query</returns>
        public ICustomerQuery ByMiddleName(string middleName)
        {
            Query = Query.Where(o => o.MiddleName == middleName);
            return this;
        }

        /// <summary>
        /// add last name filter to query
        /// </summary>
        /// <param name="lastName">customer last name</param>
        /// <returns>customer query</returns>
        public ICustomerQuery ByLastName(string lastName)
        {
            Query = Query.Where(o => o.LastName == lastName);
            return this;
        }

        /// <summary>
        /// add email filter to query
        /// </summary>
        /// <param name="email">customer email</param>
        /// <returns>customer query</returns>
        public ICustomerQuery ByEmail(string email)
        {
            Query = Query.Where(o => o.Email == email);
            return this;
        }

        /// <summary>
        /// add gender filter to query
        /// </summary>
        /// <param name="gender">customer gender</param>
        /// <returns>customer query</returns>
        public ICustomerQuery ByGender(Gender gender)
        {
            Query = Query.Where(o => (int)o.Gender == (int)gender);
            return this;
        }

        /// <summary>
        /// add phone filter to query
        /// </summary>
        /// <param name="phone">customer phone</param>
        /// <returns>customer query</returns>
        public ICustomerQuery ByPhone(string phone)
        {
            Query = Query.Where(o => o.Phone == phone);
            return this;
        }

        /// <summary>
        /// add city filter to query
        /// </summary>
        /// <param name="city">customer city</param>
        /// <returns>customer query</returns>
        public ICustomerQuery ByCity(string city)
        {
            Query = Query.Where(o => o.City == city);
            return this;
        }

        /// <summary>
        /// add country id filter to query
        /// </summary>
        /// <param name="countryId">customer country id</param>
        /// <returns>customer query</returns>
        public ICustomerQuery ByCountry(int countryId)
        {
            Query = Query.Where(o => o.CountryId == countryId);
            return this;
        }

        /// <summary>
        /// filter by custom field value
        /// </summary>
        /// <param name="customFieldName">custom field name</param>
        /// <param name="fieldValue">custom field valule</param>
        /// <returns>customer query</returns>
        public ICustomerQuery ByCustomField(string customFieldName, string fieldValue)
        {
            var customFieldQuery = _customerService.CustomFields().Where(o => o.Name == customFieldName && o.Value == fieldValue);
            Query = Query.Where(o => customFieldQuery.Any(c => c.CustomerId == o.Id));
            return this;
        }

        /// <summary>
        /// Add an address to the customer.
        /// </summary>
        /// <returns>Id of the new address.</returns>
        public int AddAddress(int customerId, Address address)
        {
            var customer = _customerService.GetById(customerId);
            var addr = CreateAddress(address);

            if (String.IsNullOrEmpty(addr.FirstName) && String.IsNullOrEmpty(addr.LastName))
            {
                addr.FirstName = customer.FirstName;
                addr.LastName = customer.LastName;
            }

            _customerService.AddAddress(customer, addr);

            address.Id = addr.Id;

            return addr.Id;
        }

        /// <summary>
        /// create the commerce object
        /// </summary>
        /// <param name="customer">commerce object</param>
        /// <returns>Id of the new created customer.</returns>
        public int Create(Customer customer)
        {
            var mapped = new Kooboo.Commerce.Customers.Customer
            {
                AccountId = customer.AccountId,
                City = customer.City,
                CountryId = customer.CountryId,
                Email = customer.Email,
                FirstName = customer.FirstName,
                LastName = customer.LastName,
                Gender = (Kooboo.Commerce.Gender)(int)customer.Gender,
                Phone = customer.Phone
            };

            foreach (var address in customer.Addresses)
            {
                mapped.Addresses.Add(CreateAddress(address));
            }

            _customerService.Create(mapped);
            customer.Id = mapped.Id;

            return mapped.Id;
        }

        private Kooboo.Commerce.Locations.Address CreateAddress(Address addr)
        {
            return new Commerce.Locations.Address
            {
                FirstName = addr.FirstName,
                LastName = addr.LastName,
                City = addr.City,
                CountryId = addr.CountryId,
                Phone = addr.Phone,
                Postcode = addr.Postcode,
                State = addr.State,
                Address1 = addr.Address1,
                Address2 = addr.Address2
            };
        }
    }
}
