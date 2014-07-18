using Kooboo.Commerce.API.Locations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.API.Customers
{
    /// <summary>
    /// customer apis
    /// </summary>
    public interface ICustomerAPI : ICustomerQuery
    {
        int Create(Customer customer);

        int AddAddress(int customerId, Address address);    
    }
}
