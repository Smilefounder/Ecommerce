using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Kooboo.CMS.Common.Runtime.Dependency;
using Kooboo.Commerce.Data;
using Kooboo.Commerce.Events;
using Kooboo.Commerce.Events.Customers;
using Kooboo.Commerce.Customers;
using Kooboo.Commerce.Locations;
using Kooboo.Commerce.Products.Services;
using Kooboo.Commerce.Payments;
using Kooboo.Commerce.ShoppingCarts;
using Kooboo.CMS.Membership.Models;
using Kooboo.Commerce.Customers.Services;
using Kooboo.Commerce.ShoppingCarts.Services;
using Kooboo.Commerce.Events.Orders;
using Kooboo.Commerce.Orders.Pricing;

namespace Kooboo.Commerce.Orders.Services
{
    [Dependency(typeof(IOrderService))]
    public class OrderService : IOrderService
    {
        private readonly ICommerceDatabase _db;
        private readonly ICustomerService _customerService;
        private readonly IRepository<Customer> _customerRepository;
        private readonly IRepository<Order> _orderRepository;
        private readonly IRepository<OrderItem> _orderItemRepository;
        private readonly IRepository<Address> _addressRepository;
        private readonly IRepository<OrderAddress> _orderAddressRepository;
        private readonly IRepository<OrderCustomField> _orderCustomFieldRepository;
        private readonly IRepository<PaymentMethod> _paymentMethodRepository;
        private readonly IRepository<Country> _countryRepository;
        private readonly IShoppingCartService _cartService;
        private readonly IProductService _productService;

        public OrderService(ICommerceDatabase db, ICustomerService customerService, IRepository<Customer> customerRepository, IRepository<Order> orderRepository, IRepository<OrderItem> orderItemRepository, IRepository<Address> addressRepository, IRepository<OrderAddress> orderAddressRepository, IRepository<OrderCustomField> orderCustomFieldRepository, IRepository<PaymentMethod> paymentMethodRepository, IRepository<Country> countryRepository, IShoppingCartService shoppingCartService, IProductService productService)
        {
            _db = db;
            _customerService = customerService;
            _customerRepository = customerRepository;
            _orderRepository = orderRepository;
            _orderItemRepository = orderItemRepository;
            _addressRepository = addressRepository;
            _orderAddressRepository = orderAddressRepository;
            _orderCustomFieldRepository = orderCustomFieldRepository;
            _paymentMethodRepository = paymentMethodRepository;
            _countryRepository = countryRepository;
            _cartService = shoppingCartService;
            _productService = productService;
        }

        public Order GetById(int id)
        {
            return _orderRepository.Find(id);
        }

        public IQueryable<Order> Query()
        {
            return _orderRepository.Query();
        }

        public IQueryable<OrderCustomField> CustomFields()
        {
            return _orderCustomFieldRepository.Query();
        }

        public Order CreateFromCart(ShoppingCart cart, ShoppingContext context)
        {
            Require.NotNull(cart, "cart");
            Require.NotNull(cart.Customer, "cart.Customer", "Cannot create order from guest cart. Login is required.");
            Require.NotNull(cart.ShippingAddress, "cart.ShippingAddress", "Shipping address is required.");
            Require.That(cart.Items.Count > 0, "cart.Items", "Cannot create order from an empty cart.");

            // Recalculate price
            var pricingContext = _cartService.CalculatePrice(cart, context);

            var order = new Order();
            order.ShoppingCartId = cart.Id;
            order.CustomerId = cart.Customer.Id;
            order.Coupon = cart.CouponCode;

            foreach (var item in pricingContext.Items)
            {
                var cartItem = cart.Items.FirstOrDefault(i => i.Id == item.ItemId);

                var orderItem = OrderItem.CreateFrom(cartItem, item.RetailPrice);
                orderItem.UnitPrice = item.RetailPrice;
                orderItem.Discount = item.Subtotal.Discount;
                orderItem.SubTotal = item.Subtotal.OriginalValue;
                orderItem.Total = item.Subtotal.FinalValue;

                order.OrderItems.Add(orderItem);
            }

            order.ShippingAddress = OrderAddress.CreateFrom(cart.ShippingAddress);

            if (cart.BillingAddress != null)
            {
                order.BillingAddress = OrderAddress.CreateFrom(cart.BillingAddress);
            }

            order.ShippingCost = pricingContext.ShippingCost.FinalValue;
            order.PaymentMethodCost = pricingContext.PaymentMethodCost.FinalValue;
            order.Tax = pricingContext.Tax.FinalValue;
            order.Discount = pricingContext.Subtotal.Discount + pricingContext.Items.Sum(x => x.Subtotal.Discount);

            order.Subtotal = pricingContext.Subtotal.FinalValue;
            order.Total = pricingContext.Total;

            Create(order);

            return order;
        }

        public bool Create(Order order)
        {
            _orderRepository.Insert(order);
            Event.Raise(new OrderCreated(order));
            return true;
        }

        public void ChangeStatus(Order order, OrderStatus newStatus)
        {
            if (order.OrderStatus != newStatus)
            {
                var oldStatus = order.OrderStatus;
                order.OrderStatus = newStatus;

                _db.SaveChanges();

                Event.Raise(new OrderStatusChanged(order, oldStatus, newStatus));
            }
        }

        public void AcceptPayment(Order order, Payment payment)
        {
            Require.NotNull(payment, "payment");
            Require.That(payment.Status == PaymentStatus.Success, "payment", "Can only accept succeeded payment.");

            order.TotalPaid += payment.Amount;

            order.Total += payment.PaymentMethodCost;
            order.PaymentMethodCost += payment.PaymentMethodCost;

            _db.SaveChanges();

            if (order.TotalPaid >= order.Total)
            {
                ChangeStatus(order, OrderStatus.Paid);
            }
        }
    }
}
