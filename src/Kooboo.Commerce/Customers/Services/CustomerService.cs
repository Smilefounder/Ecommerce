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
    [Dependency(typeof (ICustomerService))]
    public class CustomerService : ICustomerService
    {
        private readonly ICommerceDatabase _db;
        private readonly IRepository<Customer> _customerRepository;
        private readonly IRepository<CustomerLoyalty> _customerLoyaltyRepository;
        //private readonly IRepository<Order> _orderRepository;
        private readonly IRepository<Address> _addressRepository;
        //private readonly IRepository<Country> _countryRepository;

        public CustomerService(ICommerceDatabase db, IRepository<Customer> customerRepository, IRepository<Address> addressRepository, IRepository<CustomerLoyalty> customerLoyaltyRepository)
        {
            _db = db;
            _customerRepository = customerRepository;
            //_orderRepository = orderRepository;
            _addressRepository = addressRepository;
            _customerLoyaltyRepository = customerLoyaltyRepository;
            //_countryRepository = countryRepository;
        }

        //private Customer LoadAllInfo(Customer customer)
        //{
        //    if (customer != null)
        //    {
        //        customer.Addresses = _addressRepository.Query(o => o.CustomerId == customer.Id).ToList();
        //        customer.Country = _countryRepository.Query(o => o.Id == customer.CountryId).FirstOrDefault();
        //    }
        //    return customer;
        //}

        public Customer GetById(int id)
        {
            var customer = _customerRepository.Get(o => o.Id == id);
            return customer;
        }

        public IQueryable<Customer> Query()
        {
            return _customerRepository.Query();
        }

        public IQueryable<Address> QueryAddress()
        {
            return _addressRepository.Query();
        }

        public IQueryable<CustomerLoyalty> QueryCustomerLoyalty()
        {
            return _customerLoyaltyRepository.Query();
        }

        //public Customer GetByAccountId(string accountId, bool loadAllInfo = true)
        //{
        //    var customer = _customerRepository.Get(o => o.AccountId == accountId);

        //    if (loadAllInfo && customer != null)
        //    {
        //        LoadAllInfo(customer);
        //    }

        //    return customer;
        //}

        //public IPagedList<Customer> GetAllCustomers(string search, int? pageIndex, int? pageSize)
        //{
        //    var query = _customerRepository.Query();
        //    if (!string.IsNullOrEmpty(search))
        //        query = query.Where(o => o.FirstName.StartsWith(search) || o.MiddleName.StartsWith(search) || o.LastName.StartsWith(search));
        //    query = query.OrderByDescending(o => o.Id);
        //    return PageLinqExtensions.ToPagedList(query, pageIndex ?? 1, pageSize ?? 50);
        //}

        //public IPagedList<T> GetAllCustomersWithOrderCount<T>(string search, int? pageIndex, int? pageSize, Func<Customer, int, T> func)
        //{
        //    var orderQuery = _orderRepository.Query();
        //    var customerQuery = _customerRepository.Query();
        //    if (!string.IsNullOrEmpty(search))
        //        customerQuery = customerQuery.Where(o => o.FirstName.StartsWith(search) || o.MiddleName.StartsWith(search) || o.LastName.StartsWith(search));
        //    IQueryable<dynamic> query = customerQuery
        //        .GroupJoin(orderQuery,
        //                   customer => customer.Id,
        //                   order => order.CustomerId,
        //                   (customer, orders) => new { Customer = customer, Orders = orders.Count() })
        //        .OrderByDescending(groupedItem => groupedItem.Customer.Id);

        //    return PageLinqExtensions.ToPagedList<dynamic, T>(query, o => func(o.Customer, o.Orders), pageIndex ?? 1, pageSize ?? 50);
        //}

        //public IPagedList<Order> GetCustomerOrders(int customerId, int? pageIndex, int? pageSize)
        //{
        //    var orderQuery = _orderRepository.Query(o => o.CustomerId == customerId).OrderByDescending(o => o.Id);
        //    return PageLinqExtensions.ToPagedList(orderQuery, pageIndex ?? 1, pageSize ?? 50);
        //}

        public Customer CreateByAccount(MembershipUser user)
        {
            var customer = new Customer();
            customer.AccountId = user.UUID;
            customer.Email = user.Email;
            Create(customer);
            return customer;
        }

        public void Create(Customer customer)
        {
            _customerRepository.Insert(customer);
            Event.Apply(new CustomerCreated(customer));
        }

        public void Update(Customer customer)
        {
            using (var tx = _db.BeginTransaction())
            {
                if (customer.Loyalty != null)
                {
                    _customerLoyaltyRepository.Save(o => o.CustomerId == customer.Loyalty.CustomerId, customer.Loyalty, o => new object[] { o.CustomerId });
                }
                if (customer.Addresses != null)
                {
                    foreach (var address in customer.Addresses)
                    {
                        _addressRepository.Save(o => o.Id == address.Id, address, o => new object[] { o.Id });
                    }
                }
                _customerRepository.Update(customer, k => new object[] { k.Id });

                tx.Commit();
            }
        }

        public void Save(Customer customer)
        {

            if(customer.Id > 0)
            {
                bool exists = _customerRepository.Query(o => o.Id == customer.Id).Any();
                if (exists)
                    Update(customer);
                else
                    Create(customer);
            }
            else
            {
                Create(customer);
            }
        }

        public void Delete(Customer customer)
        {
            _customerRepository.Delete(customer);
        }

    }
}