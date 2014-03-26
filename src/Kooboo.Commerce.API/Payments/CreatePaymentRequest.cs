using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.API.Payments
{
    /// <summary>
    /// the request that asking for creating a payment
    /// </summary>
    public class CreatePaymentRequest
    {
        /// <summary>
        /// description
        /// </summary>
        public string Description { get; set; }
        /// <summary>
        /// the target type
        /// target is the object that payment concerns. such as: order, ticket etc.
        /// </summary>
        public string TargetType { get; set; }
        /// <summary>
        /// the target object id
        /// </summary>
        public string TargetId { get; set; }
        /// <summary>
        /// total amount that need to pay
        /// </summary>
        public decimal Amount { get; set; }
        /// <summary>
        /// payment method id
        /// </summary>
        public int PaymentMethodId { get; set; }
        /// <summary>
        /// return url when payment complete.
        /// </summary>
        public string ReturnUrl { get; set; }

        // TODO: Credit card info
    }
}
