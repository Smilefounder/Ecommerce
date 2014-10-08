using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Kooboo.CMS.Common.Runtime.Dependency;
using Kooboo.Commerce.Data;
using Kooboo.Commerce.Events;
using Kooboo.Commerce.Events.Customers;
using Kooboo.Commerce.Customers;
using Kooboo.Commerce.Countries;
using Kooboo.Commerce.Payments;
using Kooboo.Commerce.Carts;
using Kooboo.CMS.Membership.Models;
using Kooboo.Commerce.Events.Orders;
using Kooboo.Commerce.Orders.Pricing;

namespace Kooboo.Commerce.Orders
{
    [Dependency(typeof(OrderService))]
    public class OrderService
    {
        private readonly CommerceInstance _instance;
        private readonly IRepository<Order> _orderRepository;

        public OrderService(CommerceInstance instance)
        {
            _instance = instance;
            _orderRepository = _instance.Database.Repository<Order>();
        }

        public Order Find(int id)
        {
            return _orderRepository.Find(id);
        }

        public IQueryable<Order> Query()
        {
            return _orderRepository.Query();
        }

        public Order CreateFromCart(ShoppingCart cart, ShoppingContext context)
        {
            Require.NotNull(cart, "cart");
            Require.NotNull(cart.Customer, "cart.Customer", "Cannot create order from guest cart. Login is required.");
            Require.NotNull(cart.ShippingAddress, "cart.ShippingAddress", "Shipping address is required.");
            Require.That(cart.Items.Count > 0, "cart.Items", "Cannot create order from an empty cart.");

            // Recalculate price
            var pricingContext = new ShoppingCartService(_instance).CalculatePrice(cart, context);

            var order = new Order();
            order.ShoppingCartId = cart.Id;
            order.CustomerId = cart.Customer.Id;
            order.Coupon = cart.CouponCode;

            foreach (var item in pricingContext.Items)
            {
                var cartItem = cart.Items.FirstOrDefault(i => i.Id == item.ItemId);

                var orderItem = OrderItem.CreateFrom(cartItem, item.UnitPrice);
                orderItem.UnitPrice = item.UnitPrice;
                orderItem.Discount = item.Discount;
                orderItem.Subtotal = item.Subtotal;
                orderItem.Total = item.Subtotal - item.Discount;

                order.OrderItems.Add(orderItem);
            }

            order.ShippingAddress = OrderAddress.CreateFrom(cart.ShippingAddress);

            if (cart.BillingAddress != null)
            {
                order.BillingAddress = OrderAddress.CreateFrom(cart.BillingAddress);
            }

            order.ShippingCost = pricingContext.ShippingCost;
            order.PaymentMethodCost = pricingContext.PaymentMethodCost;
            order.Tax = pricingContext.Tax;
            order.Discount = pricingContext.TotalDiscount;

            order.Subtotal = pricingContext.Subtotal;
            order.Total = pricingContext.Total;

            Create(order);

            return order;
        }

        public bool Create(Order order)
        {
            _orderRepository.Insert(order);
            Event.Raise(new OrderCreated(order), new EventContext(_instance));
            return true;
        }

        public void ChangeStatus(Order order, OrderStatus newStatus)
        {
            if (order.Status != newStatus)
            {
                var oldStatus = order.Status;
                order.Status = newStatus;

                _orderRepository.Database.SaveChanges();

                Event.Raise(new OrderStatusChanged(order, oldStatus, newStatus), new EventContext(_instance));
            }
        }

        public void AcceptPayment(Order order, Payment payment)
        {
            Require.NotNull(payment, "payment");
            Require.That(payment.Status == PaymentStatus.Success, "payment", "Can only accept succeeded payment.");

            order.TotalPaid += payment.Amount;

            order.Total += payment.PaymentMethodCost;
            order.PaymentMethodCost += payment.PaymentMethodCost;

            _orderRepository.Database.SaveChanges();

            if (order.TotalPaid >= order.Total)
            {
                ChangeStatus(order, OrderStatus.Paid);
            }
        }
    }
}
