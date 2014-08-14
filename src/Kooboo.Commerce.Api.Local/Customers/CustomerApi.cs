﻿using System;
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
            var customer = _context.Services.Customers.GetById(customerId);
            var addr = CreateAddress(address);

            if (String.IsNullOrEmpty(addr.FirstName) && String.IsNullOrEmpty(addr.LastName))
            {
                addr.FirstName = customer.FirstName;
                addr.LastName = customer.LastName;
            }

            _context.Services.Customers.AddAddress(customer, addr);

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

            _context.Services.Customers.Create(mapped);
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
