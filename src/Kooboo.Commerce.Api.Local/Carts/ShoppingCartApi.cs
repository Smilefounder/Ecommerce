using Kooboo.CMS.Common.Runtime.Dependency;
using Kooboo.Commerce.Api.Customers;
using Kooboo.Commerce.Customers.Services;
using Kooboo.Commerce.Data;
using Kooboo.Commerce.Products.Services;
using Kooboo.Commerce.Promotions.Services;
using Kooboo.Commerce.Shipping.Services;
using Kooboo.Commerce.Carts.Services;
using System.Linq;
using Kooboo.Commerce.Api.Carts;

namespace Kooboo.Commerce.Api.Local.Carts
{
    /// <summary>
    /// shopping cart api
    /// </summary>
    [Dependency(typeof(IShoppingCartApi))]
    [Dependency(typeof(IShoppingCartQuery))]
    public class ShoppingCartApi : LocalCommerceQuery<ShoppingCart, Kooboo.Commerce.Carts.ShoppingCart>, IShoppingCartApi
    {
        private ICommerceDatabase _db;
        private IProductService _productService;
        private IShoppingCartService _cartService;
        private IShippingMethodService _shippingMethodService;
        private ICustomerApi _customerApi;
        private ICustomerService _customerService;
        private IPromotionService _promotionService;

        public ShoppingCartApi(LocalApiContext context, ICustomerApi customerApi)
        {
            _db = context.Database;
            _productService = context.ServiceFactory.Products;
            _customerApi = customerApi;
            _cartService = context.ServiceFactory.Carts;
            _shippingMethodService = context.ServiceFactory.ShippingMethods;
            _customerService = context.ServiceFactory.Customers;
            _promotionService = context.ServiceFactory.Promotions;

            Include(c => c.Items);
        }

        /// <summary>
        /// create entity query
        /// </summary>
        /// <returns>queryable object</returns>
        protected override IQueryable<Commerce.Carts.ShoppingCart> CreateQuery()
        {
            return _cartService.Query();
        }

        /// <summary>
        /// use the default order when pagination the query
        /// </summary>
        /// <param name="query">pagination query</param>
        /// <returns>ordered query</returns>
        protected override IQueryable<Commerce.Carts.ShoppingCart> OrderByDefault(IQueryable<Commerce.Carts.ShoppingCart> query)
        {
            return query.OrderByDescending(o => o.Id);
        }

        public IShoppingCartQuery ById(int id)
        {
            Query = Query.Where(o => o.Id == id);
            return this;
        }

        /// <summary>
        /// add session id filter to query
        /// </summary>
        /// <param name="sessionId">session id</param>
        /// <returns>shopping cart query</returns>
        public IShoppingCartQuery BySessionId(string sessionId)
        {
            Query = Query.Where(o => o.SessionId == sessionId && o.Customer == null);
            return this;
        }

        /// <summary>
        /// add account id filter to query
        /// </summary>
        /// <param name="accountId">account id</param>
        /// <returns>shopping cart query</returns>
        public IShoppingCartQuery ByAccountId(string accountId)
        {
            Query = Query.Where(o => o.Customer.AccountId == accountId);
            return this;
        }

        protected override ShoppingCart Map(Commerce.Carts.ShoppingCart obj)
        {
            Include(o => o.Items);
            Include(o => o.Items.Select(i => i.ProductPrice));

            var cart = base.Map(obj);

            // Calculate price
            var context = _cartService.CalculatePrice(obj, null);

            // Items could not empty because it might be not included
            if (cart.Items != null && cart.Items.Count > 0)
            {
                foreach (var item in context.Items)
                {
                    var cartItem = cart.Items.FirstOrDefault(x => x.Id == item.ItemId);

                    cartItem.Subtotal = item.Subtotal;
                    cartItem.Discount = item.Discount;
                    cartItem.Total = item.Subtotal - item.Discount;
                }
            }

            cart.ShippingCost = context.ShippingCost;
            cart.PaymentMethodCost = context.PaymentMethodCost;
            cart.Tax = context.Tax;

            cart.Subtotal = context.Subtotal;
            cart.TotalDiscount = context.TotalDiscount;
            cart.Total = context.Total;

            return cart;
        }

        public int CustomerCartId(string accountId)
        {
            var cart = _cartService.GetByAccountId(accountId);
            if (cart == null)
            {
                var customer = _customerService.GetByAccountId(accountId);
                cart = Kooboo.Commerce.Carts.ShoppingCart.Create(customer);
                _cartService.Create(cart);
            }

            return cart.Id;
        }

        public int SessionCartId(string sessionId)
        {
            var cart = _cartService.GetBySessionId(sessionId);
            if (cart == null)
            {
                cart = Kooboo.Commerce.Carts.ShoppingCart.Create(sessionId);
                _cartService.Create(cart);
            }

            return cart.Id;
        }

        public bool ApplyCoupon(int cartId, string coupon)
        {
            return _db.WithTransaction(() =>
            {
                var cart = _cartService.GetById(cartId);
                return _cartService.ApplyCoupon(cart, coupon);
            });
        }

        public int AddItem(int cartId, int productPriceId, int quantity)
        {
            var cart = _cartService.GetById(cartId);
            var productPrice = _productService.GetProductPriceById(productPriceId);

            return _db.WithTransaction(() =>
            {
                return _cartService.AddItem(cart, productPrice.Product, productPrice, quantity).Id;
            });
        }

        public bool RemoveItem(int cartId, int itemId)
        {
            var cart = _cartService.GetById(cartId);
            return _db.WithTransaction(() =>
            {
                return _cartService.RemoveItem(cart, itemId);
            });
        }

        public void ChangeShippingAddress(int cartId, Address address)
        {
            var cart = _cartService.GetById(cartId);

            _db.WithTransaction(() =>
            {
                var addr = GetOrCreateAddress(cart.Customer.Id, address);
                if (address.Id == 0)
                {
                    address.Id = addr.Id;
                }

                _cartService.ChangeShippingAddress(cart, addr);
            });
        }

        public void ChangeBillingAddress(int cartId, Address address)
        {
            var cart = _cartService.GetById(cartId);

            _db.WithTransaction(() =>
            {
                var addr = GetOrCreateAddress(cart.Customer.Id, address);
                if (address.Id == 0)
                {
                    address.Id = addr.Id;
                }

                _cartService.ChangeBillingAddress(cart, addr);
            });
        }

        public void ChangeShippingMethod(int cartId, int shippingMethodId)
        {
            var cart = _cartService.GetById(cartId);
            var method = _shippingMethodService.GetById(shippingMethodId);
            _db.WithTransaction(() =>
            {
                _cartService.ChangeShippingMethod(cart, method);
            });
        }

        private Kooboo.Commerce.Locations.Address GetOrCreateAddress(int customerId, Address address)
        {
            Kooboo.Commerce.Locations.Address addr = null;

            if (address.Id > 0)
            {
                addr = _customerService.Addresses().ById(address.Id);
            }
            else
            {
                _customerApi.AddAddress(customerId, address);
                addr = _customerService.Addresses().ById(address.Id);
            }

            return addr;
        }

        public void MigrateCart(int customerId, string session)
        {
            var sessionCart = _cartService.GetBySessionId(session);
            if (sessionCart == null)
            {
                return;
            }

            var customerCart = _cartService.GetByCustomer(customerId);
            if (customerCart == null)
            {
                var customer = _customerService.GetById(customerId);
                customerCart = Kooboo.Commerce.Carts.ShoppingCart.Create(customer, session);
                _cartService.Create(customerCart);
            }

            _cartService.MigrateCart(sessionCart, customerCart);
        }

        public void ChangeItemQuantity(int cartId, int itemId, int newQuantity)
        {
            var cart = _cartService.GetById(cartId);
            var item = cart.Items.FirstOrDefault(i => i.Id == itemId);
            if (item != null)
            {
                _db.WithTransaction(() =>
                {
                    _cartService.ChangeItemQuantity(cart, item, newQuantity);
                });
            }
        }

        /// <summary>
        /// expire the shopping cart, so that user can create another new shopping cart by current session id
        /// </summary>
        /// <param name="cartId">shopping cart id</param>
        /// <returns>true if successfully, else false</returns>
        public void ExpireCart(int cartId)
        {
            var shoppingCart = _cartService.Query().Where(o => o.Id == cartId).FirstOrDefault();
            if (shoppingCart != null)
            {
                _cartService.ExpireCart(shoppingCart);
            }
        }
    }
}
