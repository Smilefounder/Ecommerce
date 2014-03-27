using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.API.Payments
{
    /// <summary>
    /// payment api
    /// </summary>
    public interface IPaymentAPI : IPaymentQuery, IPaymentAccess
    {
        /// <summary>
        /// create payment query
        /// </summary>
        /// <returns>payment query</returns>
        IPaymentQuery Query();

        /// <summary>
        /// create payment data access
        /// </summary>
        /// <returns>payment data access</returns>
        IPaymentAccess Access();
    }
}
