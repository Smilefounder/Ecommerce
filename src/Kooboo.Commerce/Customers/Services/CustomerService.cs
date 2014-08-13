using System;
using System.Linq;
using Kooboo.CMS.Common.Runtime.Dependency;
using Kooboo.Commerce.Data;
using Kooboo.Commerce.Events;
using Kooboo.Commerce.Events.Customers;
using Kooboo.Commerce.Countries;

namespace Kooboo.Commerce.Customers.Services
{
    [Dependency(typeof(ICustomerService))]
    public class CustomerService : ICustomerService
    {
        private readonly IRepository<Customer> _customerRepository;
        private readonly IRepository<CustomerCustomField> _customerCustomFieldRepository;
        private readonly IRepository<Address> _addressRepository;

        public CustomerService(IRepository<Customer> customerRepository, IRepository<Address> addressRepository, IRepository<CustomerCustomField> customerCustomFieldRepository)
        {
            _customerRepository = customerRepository;
            _addressRepository = addressRepository;
            _customerCustomFieldRepository = customerCustomFieldRepository;
        }

        public Customer GetById(int id)
        {
            var customer = _customerRepository.Find(id);
            return customer;
        }

        public Customer GetByEmail(string email)
        {
            return _customerRepository.Find(o => o.Email == email);
        }

        public Customer GetByAccountId(string accountId)
        {
            if (String.IsNullOrWhiteSpace(accountId))
            {
                return null;
            }

            return _customerRepository.Find(o => o.AccountId == accountId);
        }

        public void AddAddress(Customer customer, Address address)
        {
            customer.Addresses.Add(address);
            _customerRepository.Database.SaveChanges();
        }

        public IQueryable<Customer> Query()
        {
            return _customerRepository.Query();
        }

        public IQueryable<Address> Addresses()
        {
            return _addressRepository.Query();
        }

        public void Create(Customer customer)
        {
            _customerRepository.Insert(customer);
            Event.Raise(new CustomerCreated(customer));
        }

        public void Update(Customer customer)
        {
            var dbCustomer = _customerRepository.Find(customer.Id);

            if (customer.Addresses != null)
            {
                foreach (var address in customer.Addresses)
                {
                    var dbAddress = dbCustomer.Addresses.Find(a => a.Id == address.Id);
                    if (dbAddress == null)
                    {
                        dbAddress = new Address();
                    }

                    dbAddress.FirstName = address.FirstName;
                    dbAddress.LastName = address.LastName;
                    dbAddress.Phone = address.Phone;
                    dbAddress.Postcode = address.Postcode;
                    dbAddress.Address1 = address.Address1;
                    dbAddress.Address2 = address.Address2;
                    dbAddress.City = address.City;
                    dbAddress.State = address.State;
                    dbAddress.CountryId = address.CountryId;

                    if (dbAddress.Id == 0)
                    {
                        dbCustomer.Addresses.Add(dbAddress);
                    }
                }
            }

            dbCustomer.CustomFields.Clear();

            if (customer.CustomFields != null && customer.CustomFields.Count > 0)
            {
                foreach (var field in customer.CustomFields)
                {
                    dbCustomer.CustomFields.Add(new CustomerCustomField
                    {
                        Name = field.Name,
                        Value = field.Value
                    });
                }
            }

            _customerRepository.Update(dbCustomer, customer);

            Event.Raise(new CustomerUpdated(dbCustomer));
        }

        public void Delete(Customer customer)
        {
            _customerRepository.Delete(customer);
            Event.Raise(new CustomerDeleted(customer));
        }
    }
}