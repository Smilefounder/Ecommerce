using Kooboo.CMS.Common.Runtime.Dependency;
using Kooboo.Commerce.API.Customers;
using Kooboo.Commerce.API.Locations;
using Kooboo.Commerce.Customers.Services;
using Kooboo.Commerce.Locations.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.API.LocalProvider.Customers
{
    [Dependency(typeof(ICustomerAPI), ComponentLifeStyle.Transient)]
    public class CustomerAPI : LocalCommerceQueryAccess<Customer, Kooboo.Commerce.Customers.Customer>, ICustomerAPI
    {
        private ICustomerService _customerService;
        private ICountryService _countryService;
        private IMapper<Customer, Kooboo.Commerce.Customers.Customer> _mapper;
        private IMapper<Country, Kooboo.Commerce.Locations.Country> _countryMapper;
        private IMapper<Address, Kooboo.Commerce.Locations.Address> _addressMapper;
        private IMapper<CustomerLoyalty, Kooboo.Commerce.Customers.CustomerLoyalty> _customerLoyaltyMapper;
        private bool _loadWithCountry = false;
        private bool _loadWithAddresses = false;
        private bool _loadWithCustomerLoyalty = false;

        public CustomerAPI(ICustomerService customerService, ICountryService countryService, 
            IMapper<Customer, Kooboo.Commerce.Customers.Customer> mapper, 
            IMapper<Country, Kooboo.Commerce.Locations.Country> countryMapper,
            IMapper<Address, Kooboo.Commerce.Locations.Address> addressMapper,
            IMapper<CustomerLoyalty, Kooboo.Commerce.Customers.CustomerLoyalty> customerLoyaltyMapper)
        {
            _customerService = customerService;
            _countryService = countryService;
            _mapper = mapper;
            _countryMapper = countryMapper;
            _addressMapper = addressMapper;
            _customerLoyaltyMapper = customerLoyaltyMapper;
        }

        protected override IQueryable<Kooboo.Commerce.Customers.Customer> CreateQuery()
        {
            return _customerService.Query();
        }

        protected override IQueryable<Kooboo.Commerce.Customers.Customer> OrderByDefault(IQueryable<Kooboo.Commerce.Customers.Customer> query)
        {
            return query.OrderBy(o => o.Id);
        }

        protected override Customer Map(Commerce.Customers.Customer obj)
        {
            List<string> includeComplexPropertyNames = new List<string>();
            if(_loadWithCountry)
                includeComplexPropertyNames.Add("Country");
            if(_loadWithAddresses)
                includeComplexPropertyNames.Add("Addresses");
            if(_loadWithCustomerLoyalty)
                includeComplexPropertyNames.Add("Loyalty");
            return _mapper.MapTo(obj, includeComplexPropertyNames.ToArray());
        }

        public ICustomerQuery ById(int id)
        {
            EnsureQuery();
            _query = _query.Where(o => o.Id == id);
            return this;
        }

        public ICustomerQuery ByAccountId(string accountId)
        {
            EnsureQuery();
            _query = _query.Where(o => o.AccountId == accountId);
            return this;
        }

        public ICustomerQuery ByFirstName(string firstName)
        {
            EnsureQuery();
            _query = _query.Where(o => o.FirstName == firstName);
            return this;
        }

        public ICustomerQuery ByMiddleName(string middleName)
        {
            EnsureQuery();
            _query = _query.Where(o => o.MiddleName == middleName);
            return this;
        }

        public ICustomerQuery ByLastName(string lastName)
        {
            EnsureQuery();
            _query = _query.Where(o => o.LastName == lastName);
            return this;
        }

        public ICustomerQuery ByEmail(string email)
        {
            EnsureQuery();
            _query = _query.Where(o => o.Email == email);
            return this;
        }

        public ICustomerQuery ByGender(Gender gender)
        {
            EnsureQuery();
            _query = _query.Where(o => (int)o.Gender == (int)gender);
            return this;
        }

        public ICustomerQuery ByPhone(string phone)
        {
            EnsureQuery();
            _query = _query.Where(o => o.Phone == phone);
            return this;
        }

        public ICustomerQuery ByCity(string city)
        {
            EnsureQuery();
            _query = _query.Where(o => o.City == city);
            return this;
        }

        public ICustomerQuery ByCountry(int countryId)
        {
            EnsureQuery();
            _query = _query.Where(o => o.CountryId == countryId);
            return this;
        }

        public ICustomerQuery LoadWithCountry()
        {
            _loadWithCountry = true;
            return this;
        }

        public ICustomerQuery LoadWithAddresses()
        {
            _loadWithAddresses = true;
            return this;
        }

        public ICustomerQuery LoadWithCustomerLoyalty()
        {
            _loadWithCustomerLoyalty = true;
            return this;
        }


        //private void LoadWithOptions(Customer customer)
        //{
        //    if (_loadWithCountry)
        //    {
        //        var country = _countryService.GetById(customer.CountryId.Value);
        //        if (country != null)
        //        {
        //            customer.Country = _countryMapper.MapTo(country);
        //        }
        //    }
        //    if(_loadWithAddresses)
        //    {
        //        customer.Addresses = _customerService.QueryAddress().Where(o => o.CustomerId == customer.Id).Select(o => _addressMapper.MapTo(o)).ToArray();
        //    }
        //    if(_loadWithCustomerLoyalty)
        //    {
        //        customer.Loyalty = _customerService.QueryCustomerLoyalty().Where(o => o.CustomerId == customer.Id).Select(o => _customerLoyaltyMapper.MapTo(o)).FirstOrDefault();
        //    }
        //}

        protected override void OnQueryExecuted()
        {
            _loadWithCountry = false;
            _loadWithAddresses = false;
            _loadWithCustomerLoyalty = false;
        }

        public override bool Create(Customer obj)
        {
            if (obj != null)
                return _customerService.Create(_mapper.MapFrom(obj));
            return false;
        }

        public override bool Update(Customer obj)
        {
            if (obj != null)
                return _customerService.Update(_mapper.MapFrom(obj));
            return false;
        }

        public override bool Save(Customer obj)
        {
            if (obj != null)
                return _customerService.Save(_mapper.MapFrom(obj));
            return false;
        }

        public override bool Delete(Customer obj)
        {
            if (obj != null)
                return _customerService.Delete(_mapper.MapFrom(obj));
            return false;
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
