using Kooboo.CMS.Common.Runtime.Dependency;
using Kooboo.CMS.Membership.Models;
using Kooboo.Commerce.API.Orders;
using Kooboo.Commerce.Orders.Services;
using Kooboo.Commerce.ShoppingCarts.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.API.LocalProvider.Orders
{
    [Dependency(typeof(IOrderQuery), ComponentLifeStyle.Transient)]
    public class OrderQuery : LocalCommerceQuery<Order, Kooboo.Commerce.Orders.Order>, IOrderQuery
    {
        private IOrderService _orderService;
        private IShoppingCartService _shoppingCartService;

        public OrderQuery(IOrderService orderService, IShoppingCartService shoppingCartService,
            IMapper<Order, Kooboo.Commerce.Orders.Order> mapper)
        {
            _orderService = orderService;
            _shoppingCartService = shoppingCartService;
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
            if (shoppingCart != null)
            {
                var order = _orderService.Query().Where(o => o.ShoppingCartId == shoppingCart.Id).FirstOrDefault();
                if (order == null)
                    order = _orderService.CreateOrderFromShoppingCart(shoppingCart, user, deleteShoppingCart);
                if(order != null)
                    return _mapper.MapTo(order);
            }
            return null;
        }
    }
}
