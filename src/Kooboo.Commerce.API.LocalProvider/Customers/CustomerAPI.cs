using Kooboo.CMS.Common.Runtime.Dependency;
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
        private ICommerceDatabase _db;
        private ICustomerService _customerService;
        private ICountryService _countryService;
        private IMapper<Address, Kooboo.Commerce.Locations.Address> _addressMapper;

        public CustomerAPI(
            ICommerceDatabase db,
            ICustomerService customerService, ICountryService countryService,
            IMapper<Customer, Kooboo.Commerce.Customers.Customer> mapper,
            IMapper<Address, Kooboo.Commerce.Locations.Address> addressMapper)
            : base(mapper)
        {
            _db = db;
            _customerService = customerService;
            _countryService = countryService;
            _addressMapper = addressMapper;
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
            EnsureQuery();
            _query = _query.Where(o => o.Id == id);
            return this;
        }

        /// <summary>
        /// add account id filter to query
        /// </summary>
        /// <param name="accountId">customer account id</param>
        /// <returns>customer query</returns>
        public ICustomerQuery ByAccountId(string accountId)
        {
            EnsureQuery();
            _query = _query.Where(o => o.AccountId == accountId);
            return this;
        }

        /// <summary>
        /// add first name filter to query
        /// </summary>
        /// <param name="firstName">customer first name</param>
        /// <returns>customer query</returns>
        public ICustomerQuery ByFirstName(string firstName)
        {
            EnsureQuery();
            _query = _query.Where(o => o.FirstName == firstName);
            return this;
        }

        /// <summary>
        /// add middle name filter to query
        /// </summary>
        /// <param name="middleName">customer middle name</param>
        /// <returns>customer query</returns>
        public ICustomerQuery ByMiddleName(string middleName)
        {
            EnsureQuery();
            _query = _query.Where(o => o.MiddleName == middleName);
            return this;
        }

        /// <summary>
        /// add last name filter to query
        /// </summary>
        /// <param name="lastName">customer last name</param>
        /// <returns>customer query</returns>
        public ICustomerQuery ByLastName(string lastName)
        {
            EnsureQuery();
            _query = _query.Where(o => o.LastName == lastName);
            return this;
        }

        /// <summary>
        /// add email filter to query
        /// </summary>
        /// <param name="email">customer email</param>
        /// <returns>customer query</returns>
        public ICustomerQuery ByEmail(string email)
        {
            EnsureQuery();
            _query = _query.Where(o => o.Email == email);
            return this;
        }

        /// <summary>
        /// add gender filter to query
        /// </summary>
        /// <param name="gender">customer gender</param>
        /// <returns>customer query</returns>
        public ICustomerQuery ByGender(Gender gender)
        {
            EnsureQuery();
            _query = _query.Where(o => (int)o.Gender == (int)gender);
            return this;
        }

        /// <summary>
        /// add phone filter to query
        /// </summary>
        /// <param name="phone">customer phone</param>
        /// <returns>customer query</returns>
        public ICustomerQuery ByPhone(string phone)
        {
            EnsureQuery();
            _query = _query.Where(o => o.Phone == phone);
            return this;
        }

        /// <summary>
        /// add city filter to query
        /// </summary>
        /// <param name="city">customer city</param>
        /// <returns>customer query</returns>
        public ICustomerQuery ByCity(string city)
        {
            EnsureQuery();
            _query = _query.Where(o => o.City == city);
            return this;
        }

        /// <summary>
        /// add country id filter to query
        /// </summary>
        /// <param name="countryId">customer country id</param>
        /// <returns>customer query</returns>
        public ICustomerQuery ByCountry(int countryId)
        {
            EnsureQuery();
            _query = _query.Where(o => o.CountryId == countryId);
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
            EnsureQuery();
            var customFieldQuery = _customerService.CustomFields().Where(o => o.Name == customFieldName && o.Value == fieldValue);
            _query = _query.Where(o => customFieldQuery.Any(c => c.CustomerId == o.Id));
            return this;
        }

        /// <summary>
        /// Add an address to the customer.
        /// </summary>
        /// <returns>Id of the new address.</returns>
        public int AddAddress(int customerId, Address address)
        {
            var customer = _customerService.GetById(customerId);
            var addr = _addressMapper.MapFrom(address);

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
            var mapped = _mapper.MapFrom(customer);

            foreach (var address in customer.Addresses)
            {
                mapped.Addresses.Add(_addressMapper.MapFrom(address));
            }

            _customerService.Create(mapped);
            customer.Id = mapped.Id;

            return mapped.Id;
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
