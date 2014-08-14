using Kooboo.Commerce.Api.Carts;
using Kooboo.Commerce.Api.Payments;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.Api.Orders
{
    public interface IOrderApi
    {
        Query<Order> Query();

        int CreateFromCart(int cartId, ShoppingContext context);

        PaymentResult Pay(PaymentRequest request);
    }
}
