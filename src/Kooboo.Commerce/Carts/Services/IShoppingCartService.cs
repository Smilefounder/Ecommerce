using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Kooboo.Commerce.Customers;
using Kooboo.CMS.Membership.Models;
using Kooboo.Commerce.Products;
using Kooboo.Commerce.Locations;
using Kooboo.Commerce.Orders.Pricing;
using Kooboo.Commerce.Shipping;

namespace Kooboo.Commerce.Carts.Services
{
    public interface IShoppingCartService
    {
        ShoppingCart GetById(int id);

        ShoppingCart GetBySessionId(string sessionId);

        /// <summary>
        /// 根据CMS中关联的Membership用户ID获取购物车实例。
        /// </summary>
        /// <param name="accountId"></param>
        /// <returns></returns>
        ShoppingCart GetByAccountId(string accountId);

        ShoppingCart GetByCustomer(int customerId);

        ShoppingCart GetByCustomer(string customerEmail);

        IQueryable<ShoppingCart> Query();

        void Create(ShoppingCart cart);

        PriceCalculationContext CalculatePrice(ShoppingCart cart, ShoppingContext shoppingContext);

        bool ApplyCoupon(ShoppingCart cart, string coupon);

        void AddItem(ShoppingCart cart, ShoppingCartItem item);

        ShoppingCartItem AddItem(ShoppingCart cart, Product product, ProductVariant price, int quantity);

        bool RemoveItem(ShoppingCart cart, int itemId);

        bool RemoveProduct(ShoppingCart cart, int productPriceId);

        void ChangeItemQuantity(ShoppingCart cart, ShoppingCartItem item, int newQuantity);

        void ChangeShippingAddress(ShoppingCart cart, Address address);

        void ChangeBillingAddress(ShoppingCart cart, Address address);

        void ChangeShippingMethod(ShoppingCart cart, ShippingMethod shippingMethod);

        /// <summary>
        /// 把一个购物车中的产品合并到另一个购物车中。
        /// 如果用户在未登录的状态下添加产品到购物车，然后登录，此时需要将未登录时添加的产品合并到该客户的购物车中。(参考Nop)
        /// </summary>
        void MigrateCart(ShoppingCart from, ShoppingCart to);

        void Delete(ShoppingCart cart);

        void ExpireCart(ShoppingCart cart);
    }
}