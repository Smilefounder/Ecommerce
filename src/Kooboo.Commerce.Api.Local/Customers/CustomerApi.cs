using System;
using System.Linq;
using Kooboo.Commerce.Api.Customers;

namespace Kooboo.Commerce.Api.Local.Customers
{
    public class CustomerApi : LocalCommerceQuery<Customer, Kooboo.Commerce.Customers.Customer>, ICustomerApi
    {
        public CustomerApi(LocalApiContext context)
            : base(context)
        {
        }

        protected override IQueryable<Kooboo.Commerce.Customers.Customer> CreateQuery()
        {
            return Context.Services.Customers.Query();
        }

        protected override IQueryable<Kooboo.Commerce.Customers.Customer> OrderByDefault(IQueryable<Kooboo.Commerce.Customers.Customer> query)
        {
            return query.OrderBy(o => o.Id);
        }

        public ICustomerQuery ById(int id)
        {
            Query = Query.Where(o => o.Id == id);
            return this;
        }

        public ICustomerQuery ByAccountId(string accountId)
        {
            Query = Query.Where(o => o.AccountId == accountId);
            return this;
        }

        public ICustomerQuery ByFirstName(string firstName)
        {
            Query = Query.Where(o => o.FirstName == firstName);
            return this;
        }

        public ICustomerQuery ByMiddleName(string middleName)
        {
            Query = Query.Where(o => o.MiddleName == middleName);
            return this;
        }

        public ICustomerQuery ByLastName(string lastName)
        {
            Query = Query.Where(o => o.LastName == lastName);
            return this;
        }

        public ICustomerQuery ByEmail(string email)
        {
            Query = Query.Where(o => o.Email == email);
            return this;
        }

        public ICustomerQuery ByGender(Gender gender)
        {
            Query = Query.Where(o => (int)o.Gender == (int)gender);
            return this;
        }

        public ICustomerQuery ByPhone(string phone)
        {
            Query = Query.Where(o => o.Phone == phone);
            return this;
        }

        public ICustomerQuery ByCity(string city)
        {
            Query = Query.Where(o => o.City == city);
            return this;
        }

        public ICustomerQuery ByCountry(int countryId)
        {
            Query = Query.Where(o => o.CountryId == countryId);
            return this;
        }

        public ICustomerQuery ByCustomField(string fieldName, string fieldValue)
        {
            Query = Query.Where(c => c.CustomFields.Any(f => f.Name == fieldName && f.Value == fieldValue));
            return this;
        }

        public int AddAddress(int customerId, Address address)
        {
            var customer = Context.Services.Customers.GetById(customerId);
            var addr = CreateAddress(address);

            if (String.IsNullOrEmpty(addr.FirstName) && String.IsNullOrEmpty(addr.LastName))
            {
                addr.FirstName = customer.FirstName;
                addr.LastName = customer.LastName;
            }

            Context.Services.Customers.AddAddress(customer, addr);

            address.Id = addr.Id;

            return addr.Id;
        }

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

            Context.Services.Customers.Create(mapped);
            customer.Id = mapped.Id;

            return mapped.Id;
        }

        private Kooboo.Commerce.Countries.Address CreateAddress(Address addr)
        {
            return new Commerce.Countries.Address
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
