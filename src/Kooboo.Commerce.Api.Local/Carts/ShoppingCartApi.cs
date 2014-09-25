using System.Linq;
using Kooboo.Commerce.Api.Customers;
using Kooboo.Commerce.Api.Carts;
using Core = Kooboo.Commerce.Carts;

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

        public int GetCartIdByCustomer(string email)
        {
            var service = new Core.ShoppingCartService(_context.Database);
            var cart = service.FindByCustomerEmail(email);
            if (cart == null)
            {
                var customer = new Kooboo.Commerce.Customers.CustomerService(_context.Database).FindByEmail(email);
                cart = Kooboo.Commerce.Carts.ShoppingCart.Create(customer);
                service.Create(cart);
            }

            return cart.Id;
        }

        public int GetCartIdBySessionId(string sessionId)
        {
            var service = new Core.ShoppingCartService(_context.Database);
            var cart = service.FindBySessionId(sessionId);
            if (cart == null)
            {
                cart = Kooboo.Commerce.Carts.ShoppingCart.Create(sessionId);
                service.Create(cart);
            }

            return cart.Id;
        }

        public bool ApplyCoupon(int cartId, string coupon)
        {
            return _context.Database.Transactional(() =>
            {
                var service = new Core.ShoppingCartService(_context.Database);
                var cart = service.Find(cartId);
                return service.ApplyCoupon(cart, coupon);
            });
        }

        public int AddItem(int cartId, int productVariantId, int quantity)
        {
            var database = _context.Database;
            var cartService = new Core.ShoppingCartService(database);
            var cart = cartService.Find(cartId);
            var variant = new Kooboo.Commerce.Products.ProductService(_context.Database).FindVariant(productVariantId);

            return _context.Database.Transactional(() =>
            {
                return cartService.AddItem(cart, variant.Product, variant, quantity).Id;
            });
        }

        public bool RemoveItem(int cartId, int itemId)
        {
            var service = new Core.ShoppingCartService(_context.Database);
            var cart = service.Find(cartId);
            return _context.Database.Transactional(() =>
            {
                return service.RemoveItem(cart, itemId);
            });
        }

        public void ChangeShippingAddress(int cartId, Address address)
        {
            var service = new Core.ShoppingCartService(_context.Database);
            var cart = service.Find(cartId);

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
            var service = new Core.ShoppingCartService(_context.Database);
            var cart = service.Find(cartId);

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
            var service = new Core.ShoppingCartService(_context.Database);
            var cart = service.Find(cartId);
            var method = _context.Database.Repository<Kooboo.Commerce.Shipping.ShippingMethod>().Find(shippingMethodId);
            
            _context.Database.Transactional(() =>
            {
                service.ChangeShippingMethod(cart, method);
            });
        }

        private Kooboo.Commerce.Customers.Address GetOrCreateAddress(int customerId, Address address)
        {
            var service = new Kooboo.Commerce.Customers.CustomerService(_context.Database);
            Kooboo.Commerce.Customers.Address addr = null;

            if (address.Id > 0)
            {
                addr = service.Addresses().ById(address.Id);
            }
            else
            {
                _customerApi.AddAddress(customerId, address);
                addr = service.Addresses().ById(address.Id);
            }

            return addr;
        }

        public void MigrateCart(int customerId, string session)
        {
            var service = new Core.ShoppingCartService(_context.Database);
            var sessionCart = service.FindBySessionId(session);
            if (sessionCart == null)
            {
                return;
            }

            var customerCart = service.FindByCustomerId(customerId);
            if (customerCart == null)
            {
                var customer = _context.Database.Repository<Kooboo.Commerce.Customers.Customer>().Find(customerId);
                customerCart = Kooboo.Commerce.Carts.ShoppingCart.Create(customer, session);
                service.Create(customerCart);
            }

            service.MigrateCart(sessionCart, customerCart);
        }

        public void ChangeItemQuantity(int cartId, int itemId, int newQuantity)
        {
            var service = new Core.ShoppingCartService(_context.Database);
            var cart = service.Find(cartId);
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
            var service = new Core.ShoppingCartService(_context.Database);
            var shoppingCart = service.Query().Where(o => o.Id == cartId).FirstOrDefault();
            if (shoppingCart != null)
            {
                service.ExpireCart(shoppingCart);
            }
        }
    }
}
