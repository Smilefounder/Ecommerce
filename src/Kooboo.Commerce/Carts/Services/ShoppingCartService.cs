using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Kooboo.CMS.Common.Runtime.Dependency;
using Kooboo.Commerce.Customers;
using Kooboo.Commerce.Products;
using Kooboo.Commerce.Data;
using Kooboo.Commerce.Customers.Services;
using Kooboo.CMS.Membership.Models;
using Kooboo.Commerce.Promotions;
using Kooboo.Commerce.Promotions.Services;
using Kooboo.Commerce.Orders.Pricing;
using Kooboo.Commerce.Events;
using Kooboo.Commerce.Events.Carts;
using Kooboo.Commerce.Locations;
using Kooboo.Commerce.Shipping;

namespace Kooboo.Commerce.Carts.Services
{
    [Dependency(typeof(IShoppingCartService))]
    public class ShoppingCartService : IShoppingCartService
    {
        private IRepository<ShoppingCart> _repository;
        private ICustomerService _customerService;

        public ShoppingCartService(IRepository<ShoppingCart> repository, ICustomerService customerService)
        {
            _repository = repository;
            _customerService = customerService;
        }

        public ShoppingCart GetById(int id)
        {
            var cart = _repository.Find(id);
            if (cart.SessionId != null && cart.SessionId.StartsWith("EXPIRED_"))
            {
                cart = null;
            }

            return cart;
        }

        public ShoppingCart GetBySessionId(string sessionId)
        {
            return Query().FirstOrDefault(c => c.SessionId == sessionId);
        }

        public ShoppingCart GetByAccountId(string accountId)
        {
            return Query().FirstOrDefault(c => c.Customer != null && c.Customer.AccountId == accountId);
        }

        public ShoppingCart GetByCustomer(int customerId)
        {
            return Query().FirstOrDefault(c => c.Customer != null && c.Customer.Id == customerId);
        }

        public ShoppingCart GetByCustomer(string customerEmail)
        {
            return Query().FirstOrDefault(c => c.Customer != null && c.Customer.Email == customerEmail);
        }

        public IQueryable<ShoppingCart> Query()
        {
            return _repository.Query().NotExpired();
        }

        public void Create(ShoppingCart cart)
        {
            Require.NotNull(cart, "cart");

            _repository.Insert(cart);
            Event.Raise(new CartCreated(cart));
        }

        public PriceCalculationContext CalculatePrice(ShoppingCart cart, ShoppingContext shoppingContext)
        {
            Require.NotNull(cart, "cart");

            var context = PriceCalculationContext.CreateFrom(cart);
            if (shoppingContext != null)
            {
                context.Currency = shoppingContext.Currency;
                context.Culture = shoppingContext.Culture;
            }

            new PriceCalculator().Calculate(context);

            Event.Raise(new CartPriceCalculated(cart, context));

            return context;
        }

        public bool ApplyCoupon(ShoppingCart cart, string coupon)
        {
            if (String.IsNullOrWhiteSpace(coupon))
            {
                return false;
            }

            var oldCoupon = cart.CouponCode;

            cart.CouponCode = coupon;

            var context = PriceCalculationContext.CreateFrom(cart);
            new PriceCalculator().Calculate(context);

            if (context.AppliedPromotions.Any(p => p.RequireCouponCode && p.CouponCode == coupon))
            {
                return true;
            }

            cart.CouponCode = oldCoupon;

            _repository.Database.SaveChanges();

            return false;
        }

        public void AddItem(ShoppingCart cart, ShoppingCartItem item)
        {
            Require.NotNull(cart, "cart");
            Require.NotNull(item, "item");

            cart.Items.Add(item);
            _repository.Database.SaveChanges();

            Event.Raise(new CartItemAdded(cart, item));
        }

        public ShoppingCartItem AddItem(ShoppingCart cart, Product product, ProductVariant productPrice, int quantity)
        {
            Require.NotNull(cart, "cart");
            Require.NotNull(product, "product");
            Require.NotNull(productPrice, "productPrice");
            Require.That(quantity > 0, "quantity", "Quantity should be greater than zero.");

            var item = cart.Items.FirstOrDefault(i => i.ProductVariant.Id == productPrice.Id);
            if (item == null)
            {
                item = new ShoppingCartItem(productPrice, quantity, cart);
                AddItem(cart, item);
            }
            else
            {
                ChangeItemQuantity(cart, item, item.Quantity + quantity);
            }

            return item;
        }

        public bool RemoveItem(ShoppingCart cart, int itemId)
        {
            var item = cart.Items.FirstOrDefault(i => i.Id == itemId);
            if (item == null)
            {
                return false;
            }

            cart.Items.Remove(item);
            _repository.Database.SaveChanges();

            Event.Raise(new CartItemRemoved(cart, item));

            return true;
        }

        public bool RemoveProduct(ShoppingCart cart, int productPriceId)
        {
            var item = cart.FindItemByProductPrice(productPriceId);
            if (item == null)
            {
                return false;
            }

            return RemoveItem(cart, item.Id);
        }

        public void ChangeItemQuantity(ShoppingCart cart, ShoppingCartItem item, int newQuantity)
        {
            Require.NotNull(cart, "cart");
            Require.NotNull(item, "item");
            Require.That(newQuantity > 0, "newQuantity", "Quantity should be greater than zero.");

            if (item.Quantity != newQuantity)
            {
                var oldQuantity = item.Quantity;
                item.Quantity = newQuantity;

                _repository.Database.SaveChanges();

                Event.Raise(new CartItemQuantityChanged(cart, item, oldQuantity));
            }
        }

        public void ChangeShippingAddress(ShoppingCart cart, Address address)
        {
            if (cart.ShippingAddress == null || cart.ShippingAddress.Id != address.Id)
            {
                cart.ShippingAddress = address;
                _repository.Database.SaveChanges();
            }
        }

        public void ChangeBillingAddress(ShoppingCart cart, Address address)
        {
            if (cart.BillingAddress == null || cart.BillingAddress.Id != address.Id)
            {
                cart.BillingAddress = address;
                _repository.Database.SaveChanges();
            }
        }

        public void ChangeShippingMethod(ShoppingCart cart, ShippingMethod shippingMethod)
        {
            if (cart.ShippingMethod == null || cart.ShippingMethod.Id != shippingMethod.Id)
            {
                cart.ShippingMethod = shippingMethod;
                _repository.Database.SaveChanges();
            }
        }

        public void MigrateCart(ShoppingCart from, ShoppingCart to)
        {
            Require.NotNull(from, "from");
            Require.NotNull(to, "to");

            if (from.Id == to.Id)
            {
                return;
            }

            foreach (var item in from.Items)
            {
                AddItem(to, item.ProductVariant.Product, item.ProductVariant, item.Quantity);
            }

            from.Items.Clear();
            _repository.Delete(from);
        }

        public void Delete(ShoppingCart cart)
        {
            Require.NotNull(cart, "cart");

            _repository.Delete(cart);
        }

        public void ExpireCart(ShoppingCart cart)
        {
            Require.NotNull(cart, "cart");

            cart.SessionId = string.Format("EXPIRED_{0}_{1}", cart.SessionId, DateTime.UtcNow.Ticks.ToString());
            _repository.Database.SaveChanges();

            Event.Raise(new CartExpired(cart));
        }
    }
}