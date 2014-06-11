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
        private readonly IShoppingCartService _shoppingCartService;
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
            _shoppingCartService = shoppingCartService;
            _productService = productService;
        }

        public Order GetById(int id, bool loadAllInfo = true)
        {
            var order = _orderRepository.Get(o => o.Id == id);
            if (loadAllInfo && order != null)
            {
                if (order.ShippingAddressId.HasValue)
                {
                    order.ShippingAddress = _orderAddressRepository.Query(o => o.Id == order.ShippingAddressId.Value).First();
                    order.ShippingAddress.Country = _countryRepository.Query(o => o.Id == order.ShippingAddress.CountryId).FirstOrDefault();
                }
                if (order.BillingAddressId.HasValue)
                {
                    order.BillingAddress = _orderAddressRepository.Query(o => o.Id == order.BillingAddressId.Value).First();
                    order.BillingAddress.Country = _countryRepository.Query(o => o.Id == order.BillingAddress.CountryId).FirstOrDefault();
                }
                order.OrderItems = _orderItemRepository.Query(o => o.OrderId == order.Id).ToArray();
                if (order.OrderItems != null && order.OrderItems.Count > 0)
                {
                    foreach (var item in order.OrderItems)
                    {
                        item.ProductPrice = _productService.GetProductPriceById(item.ProductPriceId, true, true);
                    }
                }
                order.Customer = _customerService.GetById(order.CustomerId);
                if (order.Customer != null)
                {
                    order.Customer.Country = _countryRepository.Query(o => o.Id == order.Customer.CountryId).FirstOrDefault();
                }
                //order.PaymentMethod = _paymentMethodRepository.Query(o => o.Id == order.PaymentMethodId).FirstOrDefault();
            }
            return order;
        }

        public IQueryable<Order> Query()
        {
            return _orderRepository.Query();
        }
        public IQueryable<OrderCustomField> CustomFieldsQuery()
        {
            return _orderCustomFieldRepository.Query();
        }

        public Order CreateFromCart(ShoppingCart cart, ShoppingContext context)
        {
            Require.NotNull(cart, "cart");
            Require.NotNull(cart.Customer, "cart.Customer", "Cannot create order from guest cart. Login is required.");
            Require.NotNull(cart.ShippingAddress, "cart.ShippingAddress", "Shipping address is required.");
            Require.That(cart.Items.Count > 0, "cart.Items", "Cannot create order from an empty cart.");

            var order = new Order();
            order.ShoppingCartId = cart.Id;
            order.CustomerId = cart.Customer.Id;
            order.Coupon = cart.CouponCode;

            var shoppingContext = context.Clone();
            shoppingContext.CustomerId = cart.Customer.Id;

            foreach (var item in cart.Items)
            {
                var orderItem = OrderItem.CreateFromCartItem(item, item.ProductPrice.RetailPrice);
                order.OrderItems.Add(orderItem);
            }

            order.ShippingAddress = OrderAddress.CreateFrom(cart.ShippingAddress);

            if (cart.BillingAddress != null)
            {
                order.BillingAddress = OrderAddress.CreateFrom(cart.BillingAddress);
            }

            // Recalculate price
            var pricingContext = new PricingContext
            {
                Currency = shoppingContext.Currency,
                CouponCode = order.Coupon,
                Culture = shoppingContext.Culture,
                Customer = cart.Customer
            };

            foreach (var item in order.OrderItems)
            {
                pricingContext.AddPricingItem(item.Id, item.ProductPrice, item.Quantity);
            }

            new PricingPipeline().Execute(pricingContext);

            foreach (var item in order.OrderItems)
            {
                var pricingItem = pricingContext.Items.FirstOrDefault(x => x.ItemId == item.Id);
                item.UnitPrice = pricingItem.RetailPrice;
                item.Discount = pricingItem.Subtotal.Discount;
                item.SubTotal = pricingItem.Subtotal.OriginalValue;
                item.Total = pricingItem.Subtotal.FinalValue;
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
            order.PaymentMethodCost += payment.PaymentMethodCost;

            _db.SaveChanges();

            if (order.TotalPaid >= order.Total)
            {
                ChangeStatus(order, OrderStatus.Paid);
            }
        }

        public bool Update(Order order)
        {
            try
            {
                var dbOrderItems = _orderItemRepository.Query(o => o.OrderId == order.Id).ToArray();
                _orderItemRepository.SaveAll(_db, dbOrderItems, order.OrderItems, k => new object[] { k.Id }, (o, n) => o.Id == n.Id);

                _orderAddressRepository.Save(o => o.Id == order.ShippingAddressId, order.ShippingAddress, k => new object[] { k.Id });
                _orderAddressRepository.Save(o => o.Id == order.BillingAddressId, order.BillingAddress, k => new object[] { k.Id });
                _orderCustomFieldRepository.DeleteBatch(o => o.OrderId == order.Id);
                if (order.CustomFields != null && order.CustomFields.Count > 0)
                {
                    foreach (var cf in order.CustomFields)
                    {
                        _orderCustomFieldRepository.Insert(cf);
                    }
                }
                _orderRepository.Update(order, k => new object[] { k.Id });
                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool Save(Order order)
        {
            if (order.Id > 0)
            {
                bool exists = _orderRepository.Query(o => o.Id == order.Id).Any();
                if (exists)
                    return Update(order);
                else
                    return Create(order);
            }
            else
            {
                return Create(order);
            }
        }

        public bool Delete(Order order)
        {
            try
            {
                _orderItemRepository.DeleteBatch(o => o.OrderId == order.Id);
                _orderAddressRepository.DeleteBatch(o => o.Id == order.ShippingAddressId);
                _orderAddressRepository.DeleteBatch(o => o.Id == order.BillingAddressId);
                _orderCustomFieldRepository.DeleteBatch(o => o.OrderId == order.Id);
                _orderRepository.Delete(order);
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
