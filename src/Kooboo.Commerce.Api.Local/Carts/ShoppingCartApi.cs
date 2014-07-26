using System.Linq;
using Kooboo.Commerce.Api.Customers;
using Kooboo.Commerce.Api.Carts;

namespace Kooboo.Commerce.Api.Local.Carts
{
    public class ShoppingCartApi : LocalCommerceQuery<ShoppingCart, Kooboo.Commerce.Carts.ShoppingCart>, IShoppingCartApi
    {
        private ICustomerApi _customerApi;

        public ShoppingCartApi(LocalApiContext context, ICustomerApi customerApi)
            : base(context)
        {
            _customerApi = customerApi;
            Include(c => c.Items);
        }

        protected override IQueryable<Commerce.Carts.ShoppingCart> CreateQuery()
        {
            return Context.Services.Carts.Query();
        }

        protected override IQueryable<Commerce.Carts.ShoppingCart> OrderByDefault(IQueryable<Commerce.Carts.ShoppingCart> query)
        {
            return query.OrderByDescending(o => o.Id);
        }

        public IShoppingCartQuery ById(int id)
        {
            Query = Query.Where(o => o.Id == id);
            return this;
        }

        public IShoppingCartQuery BySessionId(string sessionId)
        {
            Query = Query.Where(o => o.SessionId == sessionId && o.Customer == null);
            return this;
        }

        public IShoppingCartQuery ByAccountId(string accountId)
        {
            Query = Query.Where(o => o.Customer.AccountId == accountId);
            return this;
        }

        protected override ShoppingCart Map(Commerce.Carts.ShoppingCart obj)
        {
            Include(o => o.Items);
            Include(o => o.Items.Select(i => i.ProductVariant));

            var cart = base.Map(obj);

            // Calculate price
            var priceContext = Context.Services.Carts.CalculatePrice(obj, null);

            // Items could not empty because it might be not included
            if (cart.Items != null && cart.Items.Count > 0)
            {
                foreach (var item in priceContext.Items)
                {
                    var cartItem = cart.Items.FirstOrDefault(x => x.Id == item.ItemId);

                    cartItem.Subtotal = item.Subtotal;
                    cartItem.Discount = item.Discount;
                    cartItem.Total = item.Subtotal - item.Discount;
                }
            }

            cart.ShippingCost = priceContext.ShippingCost;
            cart.PaymentMethodCost = priceContext.PaymentMethodCost;
            cart.Tax = priceContext.Tax;

            cart.Subtotal = priceContext.Subtotal;
            cart.TotalDiscount = priceContext.TotalDiscount;
            cart.Total = priceContext.Total;

            return cart;
        }

        public int CustomerCartId(string accountId)
        {
            var cart = Context.Services.Carts.GetByAccountId(accountId);
            if (cart == null)
            {
                var customer = Context.Services.Customers.GetByAccountId(accountId);
                cart = Kooboo.Commerce.Carts.ShoppingCart.Create(customer);
                Context.Services.Carts.Create(cart);
            }

            return cart.Id;
        }

        public int SessionCartId(string sessionId)
        {
            var cart = Context.Services.Carts.GetBySessionId(sessionId);
            if (cart == null)
            {
                cart = Kooboo.Commerce.Carts.ShoppingCart.Create(sessionId);
                Context.Services.Carts.Create(cart);
            }

            return cart.Id;
        }

        public bool ApplyCoupon(int cartId, string coupon)
        {
            return Context.Database.WithTransaction(() =>
            {
                var service = Context.Services.Carts;
                var cart = service.GetById(cartId);
                return service.ApplyCoupon(cart, coupon);
            });
        }

        public int AddItem(int cartId, int productPriceId, int quantity)
        {
            var cartService = Context.Services.Carts;
            var cart = cartService.GetById(cartId);
            var variant = Context.Services.Products.GetProductVariantById(productPriceId);

            return Context.Database.WithTransaction(() =>
            {
                return cartService.AddItem(cart, variant.Product, variant, quantity).Id;
            });
        }

        public bool RemoveItem(int cartId, int itemId)
        {
            var service = Context.Services.Carts;
            var cart = service.GetById(cartId);
            return Context.Database.WithTransaction(() =>
            {
                return service.RemoveItem(cart, itemId);
            });
        }

        public void ChangeShippingAddress(int cartId, Address address)
        {
            var service = Context.Services.Carts;
            var cart = service.GetById(cartId);

            Context.Database.WithTransaction(() =>
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
            var service = Context.Services.Carts;
            var cart = service.GetById(cartId);

            Context.Database.WithTransaction(() =>
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
            var service = Context.Services.Carts;
            var cart = service.GetById(cartId);
            var method = Context.Services.ShippingMethods.GetById(shippingMethodId);
            
            Context.Database.WithTransaction(() =>
            {
                service.ChangeShippingMethod(cart, method);
            });
        }

        private Kooboo.Commerce.Locations.Address GetOrCreateAddress(int customerId, Address address)
        {
            Kooboo.Commerce.Locations.Address addr = null;

            if (address.Id > 0)
            {
                addr = Context.Services.Customers.Addresses().ById(address.Id);
            }
            else
            {
                _customerApi.AddAddress(customerId, address);
                addr = Context.Services.Customers.Addresses().ById(address.Id);
            }

            return addr;
        }

        public void MigrateCart(int customerId, string session)
        {
            var service = Context.Services.Carts;
            var sessionCart = service.GetBySessionId(session);
            if (sessionCart == null)
            {
                return;
            }

            var customerCart = service.GetByCustomer(customerId);
            if (customerCart == null)
            {
                var customer = Context.Services.Customers.GetById(customerId);
                customerCart = Kooboo.Commerce.Carts.ShoppingCart.Create(customer, session);
                service.Create(customerCart);
            }

            service.MigrateCart(sessionCart, customerCart);
        }

        public void ChangeItemQuantity(int cartId, int itemId, int newQuantity)
        {
            var service = Context.Services.Carts;
            var cart = service.GetById(cartId);
            var item = cart.Items.FirstOrDefault(i => i.Id == itemId);
            if (item != null)
            {
                Context.Database.WithTransaction(() =>
                {
                    service.ChangeItemQuantity(cart, item, newQuantity);
                });
            }
        }

        public void ExpireCart(int cartId)
        {
            var service = Context.Services.Carts;
            var shoppingCart = service.Query().Where(o => o.Id == cartId).FirstOrDefault();
            if (shoppingCart != null)
            {
                service.ExpireCart(shoppingCart);
            }
        }
    }
}
