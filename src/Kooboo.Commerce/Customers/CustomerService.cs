using System;
using System.Linq;
using Kooboo.CMS.Common.Runtime.Dependency;
using Kooboo.Commerce.Data;
using Kooboo.Commerce.Events;
using Kooboo.Commerce.Events.Customers;
using Kooboo.Commerce.Countries;

namespace Kooboo.Commerce.Customers
{
    [Dependency(typeof(CustomerService))]
    public class CustomerService
    {
        private readonly IRepository<Customer> _customerRepository;
        private readonly IRepository<Address> _addressRepository;

        public CustomerService(ICommerceDatabase database)
        {
            _customerRepository = database.Repository<Customer>();
            _addressRepository = database.Repository<Address>();
        }

        public Customer Find(int id)
        {
            var customer = _customerRepository.Find(id);
            return customer;
        }

        public Customer FindByEmail(string email)
        {
            return _customerRepository.Find(o => o.Email == email);
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
            if (!CanUseEmail(customer, customer.Email))
                throw new BusinessRuleViolationException("Email was already taken by others.");

            _customerRepository.Insert(customer);
            Event.Raise(new CustomerCreated(customer));
        }

        public void Update(Customer customer)
        {
            if (!CanUseEmail(customer, customer.Email))
                throw new BusinessRuleViolationException("Email was already taken by others.");

            _customerRepository.Update(customer);
            Event.Raise(new CustomerUpdated(customer));
        }

        private bool CanUseEmail(Customer customer, string email)
        {
            return !_customerRepository.Query().Any(c => c.Id != customer.Id && c.Email == email);
        }

        public void Delete(Customer customer)
        {
            _customerRepository.Delete(customer);
            Event.Raise(new CustomerDeleted(customer));
        }
    }
}