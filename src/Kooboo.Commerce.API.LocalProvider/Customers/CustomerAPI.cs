using Kooboo.CMS.Common.Runtime.Dependency;
using Kooboo.Commerce.API.Customers;
using Kooboo.Commerce.API.HAL;
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
    public class CustomerAPI : LocalCommerceQueryAccess<Customer, Kooboo.Commerce.Customers.Customer>, ICustomerAPI
    {
        private ICommerceDatabase _db;
        private ICustomerService _customerService;
        private ICountryService _countryService;
        //private IMapper<Country, Kooboo.Commerce.Locations.Country> _countryMapper;
        private IMapper<Address, Kooboo.Commerce.Locations.Address> _addressMapper;
        //private IMapper<CustomerLoyalty, Kooboo.Commerce.Customers.CustomerLoyalty> _customerLoyaltyMapper;

        public CustomerAPI(
            ICommerceDatabase db,
            IHalWrapper halWrapper, ICustomerService customerService, ICountryService countryService, 
            IMapper<Customer, Kooboo.Commerce.Customers.Customer> mapper, 
            IMapper<Country, Kooboo.Commerce.Locations.Country> countryMapper,
            IMapper<Address, Kooboo.Commerce.Locations.Address> addressMapper,
            IMapper<CustomerLoyalty, Kooboo.Commerce.Customers.CustomerLoyalty> customerLoyaltyMapper)
            : base(halWrapper, mapper)
        {
            _db = db;
            _customerService = customerService;
            _countryService = countryService;
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
            var customFieldQuery = _customerService.CustomFieldsQuery().Where(o => o.Name == customFieldName && o.Value == fieldValue);
            _query = _query.Where(o => customFieldQuery.Any(c => c.CustomerId == o.Id));
            return this;
        }

        public bool AddAddress(int customerId, Address address)
        {
            var customer = _customerService.GetById(customerId);
            var addr = _addressMapper.MapFrom(address);

            if (String.IsNullOrEmpty(addr.FirstName) && String.IsNullOrEmpty(addr.LastName))
            {
                addr.FirstName = customer.FirstName;
                addr.LastName = customer.LastName;
            }

            using (var tx = _db.BeginTransaction())
            {
                customer.Addresses.Add(addr);
                tx.Commit();

                address.Id = addr.Id;
            }

            return true;
        }

        /// <summary>
        /// create the commerce object
        /// </summary>
        /// <param name="obj">commerce object</param>
        /// <returns>true if successfully created, else false</returns>
        public override bool Create(Customer obj)
        {
            if (obj != null)
            {
                var customer = _mapper.MapFrom(obj);

                foreach (var address in obj.Addresses)
                {
                    customer.Addresses.Add(_addressMapper.MapFrom(address));
                }

                return _customerService.Create(customer);
            }

            return false;
        }

        /// <summary>
        /// update the commerce object
        /// </summary>
        /// <param name="obj">commerce object</param>
        /// <returns>true if successfully created, else false</returns>
        public override bool Update(Customer obj)
        {
            if (obj != null)
                return _customerService.Update(_mapper.MapFrom(obj));
            return false;
        }

        /// <summary>
        /// create/update the commerce object
        /// </summary>
        /// <param name="obj">commerce object</param>
        /// <returns>true if successfully created, else false</returns>
        public override bool Save(Customer obj)
        {
            if (obj != null)
                return _customerService.Save(_mapper.MapFrom(obj));
            return false;
        }

        /// <summary>
        /// delete the commerce object
        /// </summary>
        /// <param name="obj">commerce object</param>
        /// <returns>true if successfully created, else false</returns>
        public override bool Delete(Customer obj)
        {
            if (obj != null)
                return _customerService.Delete(_mapper.MapFrom(obj));
            return false;
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
