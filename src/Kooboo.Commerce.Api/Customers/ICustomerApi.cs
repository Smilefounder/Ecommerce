using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.Api.Customers
{
    public interface ICustomerApi
    {
        Query<Customer> Query();

        int Create(Customer customer);

        int AddAddress(int customerId, Address address);    
    }
}
