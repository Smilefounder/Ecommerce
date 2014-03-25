using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.API.Customers
{
    /// <summary>
    /// customer apis
    /// </summary>
    public interface ICustomerAPI : ICustomerQuery, ICustomerAccess
    {
        /// <summary>
        /// create customer query
        /// </summary>
        /// <returns>customer query</returns>
        ICustomerQuery Query();
        /// <summary>
        /// create customer data access
        /// </summary>
        /// <returns>customer data access</returns>
        ICustomerAccess Access();
    }
}
