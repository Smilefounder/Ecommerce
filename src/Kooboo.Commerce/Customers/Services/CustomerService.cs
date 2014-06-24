using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Linq.Expressions;
using Kooboo.CMS.Common.Runtime.Dependency;
using Kooboo.Commerce.Data;
using Kooboo.Commerce.Events;
using Kooboo.Commerce.Events.Customers;
using Kooboo.Commerce.Orders;
using Kooboo.Commerce.Locations;
using Kooboo.CMS.Membership.Models;

namespace Kooboo.Commerce.Customers.Services
{
    [Dependency(typeof(ICustomerService))]
    public class CustomerService : ICustomerService
    {
        private readonly ICommerceDatabase _db;
        private readonly IRepository<Customer> _customerRepository;
        private readonly IRepository<CustomerCustomField> _customerCustomFieldRepository;
        private readonly IRepository<Address> _addressRepository;

        public CustomerService(ICommerceDatabase db, IRepository<Customer> customerRepository, IRepository<Address> addressRepository, IRepository<CustomerCustomField> customerCustomFieldRepository)
        {
            _db = db;
            _customerRepository = customerRepository;
            _addressRepository = addressRepository;
            _customerCustomFieldRepository = customerCustomFieldRepository;
        }

        public Customer GetById(int id)
        {
            var customer = _customerRepository.Get(o => o.Id == id);
            return customer;
        }

        public Customer GetByEmail(string email)
        {
            return _customerRepository.Get(o => o.Email == email);
        }

        public Customer GetByAccountId(string accountId)
        {
            if (String.IsNullOrWhiteSpace(accountId))
            {
                return null;
            }

            return _customerRepository.Get(o => o.AccountId == accountId);
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

        public IQueryable<Address> QueryAddress()
        {
            return _addressRepository.Query();
        }

        public IQueryable<CustomerCustomField> CustomFieldsQuery()
        {
            return _customerCustomFieldRepository.Query();
        }

        public void Create(Customer customer)
        {
            _customerRepository.Insert(customer);
            Event.Raise(new CustomerCreated(customer));
        }

        public bool Update(Customer customer)
        {
            try
            {
                if (customer.Addresses != null)
                {
                    foreach (var address in customer.Addresses)
                    {
                        _addressRepository.Save(o => o.Id == address.Id, address, o => new object[] { o.Id });
                    }
                }

                _customerCustomFieldRepository.DeleteBatch(o => o.CustomerId == customer.Id);
                if (customer.CustomFields != null && customer.CustomFields.Count > 0)
                {
                    foreach (var cf in customer.CustomFields)
                    {
                        _customerCustomFieldRepository.Insert(new CustomerCustomField
                        {
                            CustomerId = customer.Id,
                            Name = cf.Name,
                            Value = cf.Value
                        });
                    }
                }

                _customerRepository.Update(customer, k => new object[] { k.Id });

                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool Save(Customer customer)
        {

            if (customer.Id > 0)
            {
                bool exists = _customerRepository.Query(o => o.Id == customer.Id).Any();
                if (exists)
                    return Update(customer);
                else
                {
                    Create(customer);
                    return true;
                }
            }
            else
            {
                Create(customer);
                return true;
            }
        }

        public bool Delete(Customer customer)
        {
            try
            {
                _addressRepository.DeleteBatch(o => o.CustomerId == customer.Id);
                _customerCustomFieldRepository.DeleteBatch(o => o.CustomerId == customer.Id);
                _customerRepository.Delete(customer);
                return true;
            }
            catch
            {
                return false;
            }
        }

    }
}