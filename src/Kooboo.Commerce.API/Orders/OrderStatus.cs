using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.API.Orders
{
    /// <summary>
    /// order status
    /// </summary>
    public enum OrderStatus
    {
        /// <summary>
        /// order just created
        /// </summary>
        Created = 0,
        /// <summary>
        /// waiting for user to pay
        /// </summary>
        PaymentPending = 1,
        /// <summary>
        /// user has paid the order
        /// </summary>
        Paid = 2,
        /// <summary>
        /// order is in processing
        /// </summary>
        Processing = 3,
        /// <summary>
        /// order is delivered
        /// </summary>
        Delivered = 4,
        /// <summary>
        /// order is returned, maybe need to change/replace the order product.
        /// </summary>
        Returned = 5,
        /// <summary>
        /// order is canceled, maybe need to refund.
        /// </summary>
        Cancelled = 6
    }
}
