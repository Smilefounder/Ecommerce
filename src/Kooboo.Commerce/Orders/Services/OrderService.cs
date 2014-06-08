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
        //public Order GetByShoppingCartId(int shoppingCartId)
        //{
        //    var order = _orderRepository.Query(o => o.ShoppingCartId == shoppingCartId).FirstOrDefault();
        //    return order;
        //}

        public Order CreateOrderFromShoppingCart(ShoppingCart shoppingCart, MembershipUser user, bool deleteShoppingCart)
        {
            if (shoppingCart != null)
            {
                var customer = shoppingCart.Customer;
                if (customer == null)
                {
                    customer = _customerService.Query().Where(o => o.AccountId == user.UUID).FirstOrDefault();
                    if (customer == null)
                    {
                        customer = _customerService.CreateByAccount(user);
                    }
                    shoppingCart.Customer = customer;
                    _shoppingCartService.Update(shoppingCart);
                }

                Order order = new Order();
                order.ShoppingCartId = shoppingCart.Id;
                order.CustomerId = customer.Id;
                order.IsCompleted = false;
                order.Coupon = shoppingCart.CouponCode;
                order.ChangeStatus(OrderStatus.Submitted);

                if (shoppingCart.Items.Count > 0)
                {
                    foreach (var item in shoppingCart.Items)
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

                if (shoppingCart.ShippingAddress != null)
                {
                    OrderAddress address = new OrderAddress();
                    address.FromAddress(shoppingCart.ShippingAddress);
                    order.ShippingAddress = address;
                }

                if (shoppingCart.BillingAddress != null)
                {
                    OrderAddress address = new OrderAddress();
                    address.FromAddress(shoppingCart.ShippingAddress);
                    order.BillingAddress = address;
                }

                CalculatePrice(order);

                _orderRepository.Insert(order);
                return order;
            }
            return null;
        }

        public void CalculatePrice(Order order)
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

        //public IPagedList<Order> GetAllOrders(string search, int? pageIndex, int? pageSize)
        //{
        //    var query = _orderRepository.Query();
        //    if(!string.IsNullOrEmpty(search))
        //    {
        //        int sid = 0;
        //        if (int.TryParse(search, out sid))
        //            query = query.Where(o => o.Id == sid);
        //        else
        //            query = query.Where(o => o.Customer.FirstName.StartsWith(search) || o.Customer.MiddleName.StartsWith(search) || o.Customer.LastName.StartsWith(search));
        //    }
        //    query = query.OrderByDescending(o => o.Id);
        //    return PageLinqExtensions.ToPagedList(query, pageIndex ?? 1, pageSize ?? 50);
        //}

        //public IPagedList<T> GetAllOrdersWithCustomer<T>(string search, int? pageIndex, int? pageSize, Func<Order, Customer, T> func)
        //{
        //    var customerQuery = _customerRepository.Query();
        //    var orderQuery = _orderRepository.Query();
        //    if (!string.IsNullOrEmpty(search))
        //    {
        //        int sid = 0;
        //        if (int.TryParse(search, out sid))
        //            orderQuery = orderQuery.Where(o => o.Id == sid);
        //        else
        //            customerQuery = customerQuery.Where(o => o.FirstName.StartsWith(search) || o.MiddleName.StartsWith(search) || o.LastName.StartsWith(search));
        //    }

        //    IQueryable<dynamic> query = orderQuery
        //        .Join(customerQuery,
        //                   order => order.CustomerId,
        //                   customer => customer.Id,
        //                   (order, customer) => new { Order = order, Customer = customer })
        //        .OrderByDescending(groupedItem => groupedItem.Order.Id);
        //    return PageLinqExtensions.ToPagedList<dynamic, T>(query, o => func(o.Order, o.Customer), pageIndex ?? 1, pageSize ?? 50);
        //}

        //public IPagedList<Order> GetAllCustomerOrders(int customerId, int? pageIndex, int? pageSize)
        //{
        //    var query = _orderRepository.Query(o => o.CustomerId == customerId).OrderByDescending(o => o.Id);
        //    return PageLinqExtensions.ToPagedList(query, pageIndex ?? 1, pageSize ?? 50);
        //}

        public bool Create(Order order)
        {
            return _orderRepository.Insert(order);
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
