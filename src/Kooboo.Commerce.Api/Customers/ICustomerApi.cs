using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.Api.Customers
{
    /// <summary>
    /// customer apis
    /// </summary>
    public interface ICustomerApi : ICustomerQuery
    {
        int Create(Customer customer);

        int AddAddress(int customerId, Address address);    
    }
}
