using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Kooboo.Commerce.Orders;
using Kooboo.CMS.Membership.Models;
using Kooboo.Commerce.Locations;

namespace Kooboo.Commerce.Customers.Services
{
    public interface ICustomerService
    {
        Customer GetById(int id);

        IQueryable<Customer> Query();

        IQueryable<Address> QueryAddress();

        IQueryable<CustomerLoyalty> QueryCustomerLoyalty();

        //Customer GetByAccountId(string accountId, bool loadAllInfo = true);

        //IPagedList<Customer> GetAllCustomers(string search, int? pageIndex, int? pageSize);

        //IPagedList<T> GetAllCustomersWithOrderCount<T>(string search, int? pageIndex, int? pageSize, Func<Customer, int, T> func);

        //IPagedList<Order> GetCustomerOrders(int customerId, int? pageIndex, int? pageSize);

        Customer CreateByAccount(MembershipUser user);

        bool Create(Customer customer);

        bool Update(Customer customer);

        bool Save(Customer customer);

        bool Delete(Customer customer);
    }
}