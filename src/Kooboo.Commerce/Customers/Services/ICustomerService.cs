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

        IQueryable<Customer> Query();

        IQueryable<Address> Addresses();

        void AddAddress(Customer customer, Address address);

        IQueryable<CustomerCustomField> CustomFields();

        void Create(Customer customer);

        void Update(Customer customer);

        void Delete(Customer customer);
    }
}