using System.Linq;
using Kooboo.Commerce.Api.Customers;
using Kooboo.Commerce.Api.Carts;

namespace Kooboo.Commerce.Api.Local.Carts
{
    public class ShoppingCartApi : IShoppingCartApi
    {
        private ICustomerApi _customerApi;
        private LocalApiContext _context;

        public ShoppingCartApi(LocalApiContext context, ICustomerApi customerApi)
        {
            _customerApi = customerApi;
            _context = context;
        }

        public Query<ShoppingCart> Query()
        {
            var query = new Query<ShoppingCart>(new ShoppingCartQueryExecutor(_context));
            query.Includes.Add("Items");
            return query;
        }

        public int GetCartIdByAccountId(string accountId)
        {
            var cart = _context.Services.Carts.GetByAccountId(accountId);
            if (cart == null)
            {
                var customer = _context.Services.Customers.GetByAccountId(accountId);
                cart = Kooboo.Commerce.Carts.ShoppingCart.Create(customer);
                _context.Services.Carts.Create(cart);
            }

            return cart.Id;
        }

        public int GetCartIdBySessionId(string sessionId)
        {
            var cart = _context.Services.Carts.GetBySessionId(sessionId);
            if (cart == null)
            {
                cart = Kooboo.Commerce.Carts.ShoppingCart.Create(sessionId);
                _context.Services.Carts.Create(cart);
            }

            return cart.Id;
        }

        public bool ApplyCoupon(int cartId, string coupon)
        {
            return _context.Database.Transactional(() =>
            {
                var service = _context.Services.Carts;
                var cart = service.GetById(cartId);
                return service.ApplyCoupon(cart, coupon);
            });
        }

        public int AddItem(int cartId, int productPriceId, int quantity)
        {
            var cartService = _context.Services.Carts;
            var cart = cartService.GetById(cartId);
            var variant = _context.Services.Products.GetProductVariantById(productPriceId);

            return _context.Database.Transactional(() =>
            {
                return cartService.AddItem(cart, variant.Product, variant, quantity).Id;
            });
        }

        public bool RemoveItem(int cartId, int itemId)
        {
            var service = _context.Services.Carts;
            var cart = service.GetById(cartId);
            return _context.Database.Transactional(() =>
            {
                return service.RemoveItem(cart, itemId);
            });
        }

        public void ChangeShippingAddress(int cartId, Address address)
        {
            var service = _context.Services.Carts;
            var cart = service.GetById(cartId);

            _context.Database.Transactional(() =>
            {
                var addr = GetOrCreateAddress(cart.Customer.Id, address);
                if (address.Id == 0)
                {
                    address.Id = addr.Id;
                }

                service.ChangeShippingAddress(cart, addr);
            });
        }

        public void ChangeBillingAddress(int cartId, Address address)
        {
            var service = _context.Services.Carts;
            var cart = service.GetById(cartId);

            _context.Database.Transactional(() =>
            {
                var addr = GetOrCreateAddress(cart.Customer.Id, address);
                if (address.Id == 0)
                {
                    address.Id = addr.Id;
                }

                service.ChangeBillingAddress(cart, addr);
            });
        }

        public void ChangeShippingMethod(int cartId, int shippingMethodId)
        {
            var service = _context.Services.Carts;
            var cart = service.GetById(cartId);
            var method = _context.Services.ShippingMethods.GetById(shippingMethodId);
            
            _context.Database.Transactional(() =>
            {
                service.ChangeShippingMethod(cart, method);
            });
        }

        private Kooboo.Commerce.Customers.Address GetOrCreateAddress(int customerId, Address address)
        {
            Kooboo.Commerce.Customers.Address addr = null;

            if (address.Id > 0)
            {
                addr = _context.Services.Customers.Addresses().ById(address.Id);
            }
            else
            {
                _customerApi.AddAddress(customerId, address);
                addr = _context.Services.Customers.Addresses().ById(address.Id);
            }

            return addr;
        }

        public void MigrateCart(int customerId, string session)
        {
            var service = _context.Services.Carts;
            var sessionCart = service.GetBySessionId(session);
            if (sessionCart == null)
            {
                return;
            }

            var customerCart = service.GetByCustomer(customerId);
            if (customerCart == null)
            {
                var customer = _context.Services.Customers.GetById(customerId);
                customerCart = Kooboo.Commerce.Carts.ShoppingCart.Create(customer, session);
                service.Create(customerCart);
            }

            service.MigrateCart(sessionCart, customerCart);
        }

        public void ChangeItemQuantity(int cartId, int itemId, int newQuantity)
        {
            var service = _context.Services.Carts;
            var cart = service.GetById(cartId);
            var item = cart.Items.FirstOrDefault(i => i.Id == itemId);
            if (item != null)
            {
                _context.Database.Transactional(() =>
                {
                    service.ChangeItemQuantity(cart, item, newQuantity);
                });
            }
        }

        public void ExpireCart(int cartId)
        {
            var service = _context.Services.Carts;
            var shoppingCart = service.Query().Where(o => o.Id == cartId).FirstOrDefault();
            if (shoppingCart != null)
            {
                service.ExpireCart(shoppingCart);
            }
        }
    }
}
