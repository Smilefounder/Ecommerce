using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Kooboo.Commerce.Customers;
using Kooboo.Commerce.ShoppingCarts;
using Kooboo.CMS.Membership.Models;

namespace Kooboo.Commerce.Orders.Services
{
    public interface IOrderService
    {
        Order GetById(int id, bool loadAllInfo = true);

        IQueryable<Order> Query();

        IQueryable<OrderCustomField> CustomFieldsQuery();

        //Order GetByShoppingCartId(int shoppingCartId);

        Order CreateOrderFromShoppingCart(ShoppingCart shoppingCart, MembershipUser user, bool deleteShoppingCart);

        //IPagedList<Order> GetAllOrders(string search, int? pageIndex, int? pageSize);

        //IPagedList<T> GetAllOrdersWithCustomer<T>(string search, int? pageIndex, int? pageSize, Func<Order, Customer, T> func);

        //IPagedList<Order> GetAllCustomerOrders(int customerId, int? pageIndex, int? pageSize);

        bool Create(Order order);

        bool Update(Order order);

        bool Save(Order order);

        bool Delete(Order order);
    }
}
