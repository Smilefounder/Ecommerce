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

        public Order CreateFromCart(ShoppingCart cart, MembershipUser user, bool deleteShoppingCart)
        {
            Require.NotNull(cart, "cart");

            var customer = cart.Customer;
            if (customer == null)
            {
                customer = _customerService.Query().Where(o => o.AccountId == user.UUID).FirstOrDefault();
                if (customer == null)
                {
                    customer = _customerService.CreateByAccount(user);
                }
                cart.Customer = customer;

                _db.SaveChanges();
            }

            var order = new Order();
            order.ShoppingCartId = cart.Id;
            order.CustomerId = customer.Id;
            order.IsCompleted = false;
            order.Coupon = cart.CouponCode;

            if (cart.Items.Count > 0)
            {
                foreach (var item in cart.Items)
                {
                    var orderItem = new OrderItem();
                    orderItem.Order = order;
                    orderItem.ProductPriceId = item.ProductPrice.Id;
                    orderItem.ProductPrice = item.ProductPrice;
                    orderItem.ProductName = item.ProductPrice.Name;
                    orderItem.SKU = item.ProductPrice.Sku;
                    orderItem.UnitPrice = item.ProductPrice.RetailPrice;
                    orderItem.Quantity = item.Quantity;
                    order.OrderItems.Add(orderItem);
                }
            }

            if (cart.ShippingAddress != null)
            {
                OrderAddress address = new OrderAddress();
                address.FromAddress(cart.ShippingAddress);
                order.ShippingAddress = address;
            }

            if (cart.BillingAddress != null)
            {
                OrderAddress address = new OrderAddress();
                address.FromAddress(cart.ShippingAddress);
                order.BillingAddress = address;
            }

            CalculatePrice(order);

            _orderRepository.Insert(order);
            return order;
        }

        private void CalculatePrice(Order order)
        {
            var context = PricingContext.CreateFrom(order);
            new PricingPipeline().Execute(context);

            foreach (var item in order.OrderItems)
            {
                var pricingItem = context.Items.FirstOrDefault(x => x.ItemId == item.Id);
                item.Discount = pricingItem.Subtotal.Discount;
                item.SubTotal = pricingItem.Subtotal.OriginalValue;
                item.Total = pricingItem.Subtotal.FinalValue;
            }

            order.SubTotal = context.Subtotal.FinalValue;
            order.ShippingCost = context.ShippingCost.FinalValue;
            order.PaymentMethodCost = context.PaymentMethodCost.FinalValue;
            order.TotalTax = context.Tax.FinalValue;
            order.Discount = context.Subtotal.Discount + context.Items.Sum(x => x.Subtotal.Discount);

            order.Total = context.Total;
        }

        public bool Create(Order order)
        {
            return _orderRepository.Insert(order);
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
