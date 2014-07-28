using System;
using System.Linq;
using Kooboo.Commerce.Api.Orders;
using Kooboo.Commerce.Api.Carts;

namespace Kooboo.Commerce.Api.Local.Orders
{
    public class OrderApi : LocalCommerceQuery<Order, Kooboo.Commerce.Orders.Order>, IOrderApi
    {
        public OrderApi(LocalApiContext context)
            : base(context)
        {
        }

        protected override IQueryable<Commerce.Orders.Order> CreateQuery()
        {
            return Context.Services.Orders.Query();
        }

        protected override IQueryable<Commerce.Orders.Order> OrderByDefault(IQueryable<Commerce.Orders.Order> query)
        {
            return query.OrderByDescending(o => o.Id);
        }

        public int CreateFromCart(int cartId, ShoppingContext context)
        {
            var cart = Context.Services.Carts.GetById(cartId);

            return Context.Database.WithTransaction(() =>
            {
                var order = Context.Services.Orders.CreateFromCart(cart, new Kooboo.Commerce.Carts.ShoppingContext
                {
                    Culture = context.Culture,
                    Currency = context.Currency,
                    CustomerId = context.CustomerId
                });

                return order.Id;
            });
        }

        public IOrderQuery ById(int id)
        {
            Query = Query.Where(o => o.Id == id);
            return this;
        }

        public IOrderQuery ByCustomerId(int customerId)
        {
            Query = Query.Where(o => o.CustomerId == customerId);
            return this;
        }

        public IOrderQuery ByAccountId(string accountId)
        {
            Query = Query.Where(o => o.Customer.AccountId == accountId);
            return this;
        }

        public IOrderQuery ByCreateDate(DateTime? from, DateTime? to)
        {
            if (from.HasValue)
                Query = Query.Where(o => o.CreatedAtUtc >= from.Value);
            if (to.HasValue)
                Query = Query.Where(o => o.CreatedAtUtc <= to.Value);
            return this;
        }

        public IOrderQuery ByOrderStatus(OrderStatus status)
        {
            Query = Query.Where(o => (int)o.Status == (int)status);
            return this;
        }

        public IOrderQuery ByCoupon(string coupon)
        {
            Query = Query.Where(o => o.Coupon == coupon);
            return this;
        }

        public IOrderQuery ByTotal(decimal? from, decimal? to)
        {
            if (from.HasValue)
                Query = Query.Where(o => o.Total >= from.Value);
            if (to.HasValue)
                Query = Query.Where(o => o.Total <= to.Value);
            return this;
        }

        public IOrderQuery ByCustomField(string fieldName, string fieldValue)
        {
            Query = Query.Where(o => o.CustomFields.Any(f => f.Name == fieldName && f.Value == fieldValue));
            return this;
        }
    }
}
