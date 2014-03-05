using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Kooboo.CMS.Common.Runtime.Dependency;
using Kooboo.Commerce.Data;
using Kooboo.Commerce.Events;
using Kooboo.Commerce.Events.Customers;
using Kooboo.Commerce.Customers;
using Kooboo.Web.Mvc.Paging;
using Kooboo.Commerce.Locations;
using Kooboo.Commerce.Products.Services;
using Kooboo.Commerce.Payments;
using Kooboo.Commerce.Accounts;
using Kooboo.Commerce.ShoppingCarts;

namespace Kooboo.Commerce.Orders.Services
{
    [Dependency(typeof(IOrderService))]
    public class OrderService : IOrderService
    {
        private readonly ICommerceDatabase _db;
        private readonly IRepository<Customer> _customerRepository;
        private readonly IRepository<Order> _orderRepository;
        private readonly IRepository<OrderItem> _orderItemRepository;
        private readonly IRepository<Address> _addressRepository;
        private readonly IRepository<OrderAddress> _orderAddressRepository;
        private readonly IRepository<PaymentMethod> _paymentMethodRepository;
        private readonly IRepository<Country> _countryRepository;
        private readonly IRepository<Account> _accountRepository;
        private readonly IRepository<ShoppingCart> _shoppingCartRepository;
        private readonly ProductService _productService;

        public OrderService(ICommerceDatabase db, IRepository<Customer> customerRepository, IRepository<Order> orderRepository, IRepository<OrderItem> orderItemRepository, IRepository<Address> addressRepository, IRepository<OrderAddress> orderAddressRepository, IRepository<PaymentMethod> paymentMethodRepository, IRepository<Country> countryRepository, IRepository<Account> accountRepository, IRepository<ShoppingCart> shoppingCartRepository, ProductService productService)
        {
            _db = db;
            _customerRepository = customerRepository;
            _orderRepository = orderRepository;
            _orderItemRepository = orderItemRepository;
            _addressRepository = addressRepository;
            _orderAddressRepository = orderAddressRepository;
            _paymentMethodRepository = paymentMethodRepository;
            _countryRepository = countryRepository;
            _accountRepository = accountRepository;
            _shoppingCartRepository = shoppingCartRepository;
            _productService = productService;
        }

        public Order GetById(int id, bool loadAllInfo = true)
        {
            var order = _orderRepository.Get(o => o.Id == id);
            if(loadAllInfo && order != null)
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
                if(order.OrderItems != null && order.OrderItems.Count > 0)
                {
                    foreach(var item in order.OrderItems)
                    {
                        item.ProductPrice = _productService.GetProductPriceById(item.ProductPriceId, true, true);
                    }
                }
                order.Customer = _customerRepository.Query(o => o.Id == order.CustomerId).FirstOrDefault();
                if(order.Customer != null)
                {
                    order.Customer.Country = _countryRepository.Query(o => o.Id == order.Customer.CountryId).FirstOrDefault();
                    order.Customer.Account = _accountRepository.Query(o => o.Id == order.Customer.AccountId.Value).FirstOrDefault();
                }
                order.PaymentMethod = _paymentMethodRepository.Query(o => o.Id == order.PaymentMethodId).FirstOrDefault();
            }
            return order;
        }

        public Order CreateOrderFromShoppingCart(int shoppingCartId)
        {
            var shoppingCart = _shoppingCartRepository.Query(o => o.Id == shoppingCartId).FirstOrDefault();
            if(shoppingCart != null)
            {
                Order order = new Order();
                order.ShoppingCartId = shoppingCart.Id;
                order.CustomerId = shoppingCart.Customer.Id;
                order.IsCompleted = false;
                order.ChangeOrderStatus(OrderStatus.Created);

                if(shoppingCart.Items.Count > 0)
                {
                    foreach(var item in shoppingCart.Items)
                    {
                        var orderItem = new OrderItem();
                        orderItem.Order = order;
                        orderItem.ProductPriceId = item.ProductPrice.Id;
                        orderItem.ProductName = item.ProductPrice.Name;
                        orderItem.SKU = item.ProductPrice.Sku;
                        orderItem.UnitPrice = item.ProductPrice.RetailPrice;
                        orderItem.Quantity = item.Quantity;
                        orderItem.SubTotal = orderItem.UnitPrice * orderItem.Quantity;
                        orderItem.Discount = 0m;
                        orderItem.TaxCost = 0m;
                        orderItem.Total = orderItem.SubTotal - orderItem.Discount + orderItem.TaxCost;

                        order.OrderItems.Add(orderItem);
                    }
                }

                if(shoppingCart.ShippingAddress != null)
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

                _orderRepository.Insert(order);
                return order;
            }
            return null;
        }

        public IPagedList<Order> GetAllOrders(string search, int? pageIndex, int? pageSize)
        {
            var query = _orderRepository.Query();
            if(!string.IsNullOrEmpty(search))
            {
                int sid = 0;
                if (int.TryParse(search, out sid))
                    query = query.Where(o => o.Id == sid);
                else
                    query = query.Where(o => o.Customer.FirstName.StartsWith(search) || o.Customer.MiddleName.StartsWith(search) || o.Customer.LastName.StartsWith(search));
            }
            query = query.OrderByDescending(o => o.Id);
            return PageLinqExtensions.ToPagedList(query, pageIndex ?? 1, pageSize ?? 50);
        }

        public IPagedList<T> GetAllOrdersWithCustomer<T>(string search, int? pageIndex, int? pageSize, Func<Order, Customer, T> func)
        {
            int pi = pageIndex ?? 1;
            int ps = pageSize ?? 50;
            pi = pi < 1 ? 1 : pi;
            ps = ps < 1 ? 50 : ps;
            var customerQuery = _customerRepository.Query();
            var orderQuery = _orderRepository.Query();
            if (!string.IsNullOrEmpty(search))
            {
                int sid = 0;
                if (int.TryParse(search, out sid))
                    orderQuery = orderQuery.Where(o => o.Id == sid);
                else
                    customerQuery = customerQuery.Where(o => o.FirstName.StartsWith(search) || o.MiddleName.StartsWith(search) || o.LastName.StartsWith(search));
            }

            IQueryable<dynamic> query = orderQuery
                .Join(customerQuery,
                           order => order.CustomerId,
                           customer => customer.Id,
                           (order, customer) => new { Order = order, Customer = customer })
                .OrderByDescending(groupedItem => groupedItem.Order.Id);
            var total = query.Count();
            var data = query.Skip(ps * (pi - 1)).Take(ps).ToArray();
            return new PagedList<T>(data.Select<dynamic, T>(o => func(o.Order, o.Customer)), pi, ps, total);
        }

        public void Create(Order order)
        {
            _orderRepository.Insert(order);
            //Event.Apply(new OrderCreated(order));
        }

        public void Update(Order order)
        {
            using (var tx = _db.BeginTransaction())
            {
                _orderRepository.Update(order, k => new object[] { k.Id });

                var dbOrderItems = _orderItemRepository.Query(o => o.OrderId == order.Id).ToArray();
                _orderItemRepository.SaveAll(_db, dbOrderItems, order.OrderItems, k => new object[] { k.Id }, (o, n) => o.Id == n.Id);

                _orderAddressRepository.Save(o => o.Id == order.ShippingAddressId, order.ShippingAddress, k => new object[] { k.Id });
                _orderAddressRepository.Save(o => o.Id == order.BillingAddressId, order.BillingAddress, k => new object[] { k.Id });

                tx.Commit();
            }
        }

        public void Save(Order order)
        {
            if (order.Id > 0)
            {
                bool exists = _orderRepository.Query(o => o.Id == order.Id).Any();
                if (exists)
                    Update(order);
                else
                    Create(order);
            }
            else
            {
                Create(order);
            }
        }

        public void Delete(Order order)
        {
            _orderRepository.Delete(order);
        }
    }
}
