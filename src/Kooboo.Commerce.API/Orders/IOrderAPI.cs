using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.API.Orders
{
    /// <summary>
    /// order api
    /// </summary>
    public interface IOrderAPI : IOrderQuery, IOrderAccess
    {
        /// <summary>
        /// create order query
        /// </summary>
        /// <returns>order query</returns>
        IOrderQuery Query();
        /// <summary>
        /// create order data access
        /// </summary>
        /// <returns>order data access</returns>
        IOrderAccess Access();
    }
}
