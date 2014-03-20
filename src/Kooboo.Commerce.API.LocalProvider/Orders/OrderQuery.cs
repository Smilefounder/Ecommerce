using Kooboo.CMS.Common.Runtime.Dependency;
using Kooboo.CMS.Membership.Models;
using Kooboo.Commerce.API.Orders;
using Kooboo.Commerce.Customers.Services;
using Kooboo.Commerce.Orders.Services;
using Kooboo.Commerce.ShoppingCarts.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.API.LocalProvider.Orders
{
    [Dependency(typeof(IOrderQuery), ComponentLifeStyle.Transient)]
    public class OrderQuery : LocalCommerceQueryAccess<Order, Kooboo.Commerce.Orders.Order>, IOrderQuery
    {
        private IOrderService _orderService;
        private IShoppingCartService _shoppingCartService;
        private ICustomerService _customerService;
        private IMapper<Order, Kooboo.Commerce.Orders.Order> _mapper;
        private bool _loadWithCustomer = false;
        private bool _loadWithShoppingCart = false;

        public OrderQuery(IOrderService orderService, IShoppingCartService shoppingCartService, ICustomerService customerService,
            IMapper<Order, Kooboo.Commerce.Orders.Order> mapper)
        {
            _orderService = orderService;
            _shoppingCartService = shoppingCartService;
            _customerService = customerService;
            _mapper = mapper;
        }

        protected override IQueryable<Commerce.Orders.Order> CreateQuery()
        {
            return _orderService.Query();
        }

        protected override IQueryable<Commerce.Orders.Order> OrderByDefault(IQueryable<Commerce.Orders.Order> query)
        {
            return query.OrderByDescending(o => o.Id);
        }

        protected override Order Map(Commerce.Orders.Order obj)
        {
            List<string> includeComplexPropertyNames = new List<string>();
            includeComplexPropertyNames.Add("OrderItem");
            includeComplexPropertyNames.Add("OrderItem.ProductPrice");
            includeComplexPropertyNames.Add("OrderItem.ProductPrice.Product");
            includeComplexPropertyNames.Add("OrderItem.ProductPrice.ProductPriceVariantValue");
            includeComplexPropertyNames.Add("OrderItem.ProductPrice.ProductPriceVariantValue.ProductPrice");
            includeComplexPropertyNames.Add("OrderItem.ProductPrice.ProductPriceVariantValue.CustomField");
            includeComplexPropertyNames.Add("ShippingAddress");
            includeComplexPropertyNames.Add("ShippingAddress.Country");
            includeComplexPropertyNames.Add("BillingAddress");
            includeComplexPropertyNames.Add("BillingAddress.Country");
            includeComplexPropertyNames.Add("PaymentMethod");
            if (_loadWithCustomer)
            {
                includeComplexPropertyNames.Add("Customer");
            }
            if(_loadWithShoppingCart)
            {
                includeComplexPropertyNames.Add("ShoppingCart");
                includeComplexPropertyNames.Add("ShoppingCart.Items");
                includeComplexPropertyNames.Add("ShoppingCart.Items.ProductPrice");
                includeComplexPropertyNames.Add("ShoppingCart.Items.ProductPrice.Product");
                includeComplexPropertyNames.Add("ShoppingCart.ShippingAddress");
                includeComplexPropertyNames.Add("ShoppingCart.BillingAddress");
            }

            return _mapper.MapTo(obj, includeComplexPropertyNames.ToArray());
        }

        public IOrderQuery LoadWithCustomer()
        {
            _loadWithCustomer = true;
            return this;
        }
        public IOrderQuery LoadWithShoppingCart()
        {
            _loadWithShoppingCart = true;
            return this;
        }
        protected override void OnQueryExecuted()
        {
            _loadWithCustomer = false;
            _loadWithShoppingCart = false;
        }

        public override bool Create(Order obj)
        {
            if(obj != null)
            {
                return _orderService.Create(_mapper.MapFrom(obj));
            }
            return false;
        }

        public override bool Update(Order obj)
        {
            if (obj != null)
            {
                return _orderService.Update(_mapper.MapFrom(obj));
            }
            return false;
        }

        public override bool Save(Order obj)
        {
            if (obj != null)
            {
                return _orderService.Save(_mapper.MapFrom(obj));
            }
            return false;
        }

        public override bool Delete(Order obj)
        {
            if (obj != null)
            {
                return _orderService.Delete(_mapper.MapFrom(obj));
            }
            return false;
        }

        public IOrderQuery ById(int id)
        {
            EnsureQuery();
            _query = _query.Where(o => o.Id == id);
            return this;
        }

        public IOrderQuery ByCustomerId(int customerId)
        {
            EnsureQuery();
            _query = _query.Where(o => o.CustomerId == customerId);
            return this;
        }

        public IOrderQuery ByAccountId(string accountId)
        {
            EnsureQuery();
            _query = _query.Where(o => o.Customer.AccountId == accountId);
            return this;
        }

        public Order GetMyOrder(string sessionId, MembershipUser user, bool deleteShoppingCart = true)
        {
            var shoppingCart = string.IsNullOrEmpty(sessionId) ? null : _shoppingCartService.Query().Where(o => o.SessionId == sessionId).FirstOrDefault();
            Kooboo.Commerce.Orders.Order order = null;
            if (shoppingCart != null)
            {
                order = _orderService.Query().Where(o => o.ShoppingCartId == shoppingCart.Id).FirstOrDefault();
                if (order == null)
                    order = _orderService.CreateOrderFromShoppingCart(shoppingCart, user, deleteShoppingCart);
            }
            else
            {
                var customer = _customerService.Query().Where(o => o.AccountId == user.UUID).FirstOrDefault();
                if (customer != null)
                {
                    order = _orderService.Query().Where(o => o.CustomerId == customer.Id && o.OrderStatus == Commerce.Orders.OrderStatus.Created).OrderByDescending(o => o.CreatedAtUtc).FirstOrDefault();
                }
            }
            if (order != null)
            {
                LoadWithCustomer();
                var morder = Map(order);
                OnQueryExecuted();
                return morder;
            }
            return null;
        }
    }
}
