using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Kooboo.Commerce.Customers;
using Kooboo.Commerce.ShoppingCarts;
using Kooboo.Commerce.Payments;

namespace Kooboo.Commerce.Orders.Services
{
    public interface IOrderService
    {
        Order GetById(int id);

        IQueryable<Order> Query();

        IQueryable<OrderCustomField> CustomFields();

        Order CreateFromCart(ShoppingCart cart, ShoppingContext context);

        bool Create(Order order);

        void AcceptPayment(Order order, Payment payment);

        void ChangeStatus(Order order, OrderStatus newStatus);
    }
}
