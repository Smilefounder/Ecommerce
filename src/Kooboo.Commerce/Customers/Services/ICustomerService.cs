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

        Customer GetByEmail(string email);

        Customer GetByAccountId(string accountId);

        void AddAddress(Customer customer, Address address);

        IQueryable<Customer> Query();

        IQueryable<Address> QueryAddress();

        IQueryable<CustomerCustomField> CustomFieldsQuery();

        Customer CreateByAccount(MembershipUser user);

        bool Create(Customer customer);

        bool Update(Customer customer);

        bool Save(Customer customer);

        bool Delete(Customer customer);
    }
}