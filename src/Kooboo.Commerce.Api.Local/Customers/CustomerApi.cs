using System;
using System.Linq;
using Kooboo.Commerce.Api.Customers;

namespace Kooboo.Commerce.Api.Local.Customers
{
    public class CustomerApi : ICustomerApi
    {
        private LocalApiContext _context;

        public CustomerApi(LocalApiContext context)
        {
            _context = context;
        }

        public Query<Customer> Query()
        {
            return new Query<Customer>(new CustomerQueryExecutor(_context));
        }

        public int AddAddress(int customerId, Address address)
        {
            var service = new Kooboo.Commerce.Customers.CustomerService(_context.Database);
            var customer = service.Find(customerId);
            var addr = CreateAddress(address);

            if (String.IsNullOrEmpty(addr.FirstName) && String.IsNullOrEmpty(addr.LastName))
            {
                addr.FirstName = customer.FirstName;
                addr.LastName = customer.LastName;
            }

            service.AddAddress(customer, addr);

            address.Id = addr.Id;

            return addr.Id;
        }

        public int Create(Customer model)
        {
            var customer = new Kooboo.Commerce.Customers.Customer
            {
                Email = model.Email,
                FirstName = model.FirstName,
                LastName = model.LastName,
                Gender = (Kooboo.Commerce.Gender)(int)model.Gender
            };

            foreach (var address in model.Addresses)
            {
                customer.Addresses.Add(CreateAddress(address));
            }

            new Kooboo.Commerce.Customers.CustomerService(_context.Database).Create(customer);

            model.Id = customer.Id;

            return customer.Id;
        }

        private Kooboo.Commerce.Customers.Address CreateAddress(Address addr)
        {
            return new Commerce.Customers.Address
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

        public void SetDefaultShippingAddress(int customerId, int addressId)
        {
            var customer = _context.Database.Repository<Customer>().Find(customerId);
            customer.DefaultShippingAddressId = addressId;
            _context.Database.SaveChanges();
        }

        public void SetDefaultBillingAddress(int customerId, int addressId)
        {
            var customer = _context.Database.Repository<Customer>().Find(customerId);
            customer.DefaultBillingAddressId = addressId;
            _context.Database.SaveChanges();
        }
    }
}
