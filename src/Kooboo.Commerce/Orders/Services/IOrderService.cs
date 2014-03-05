using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Kooboo.Commerce.Customers;

namespace Kooboo.Commerce.Orders.Services
{
    public interface IOrderService
    {
        Order GetById(int id, bool loadAllInfo = true);

        Order CreateOrderFromShoppingCart(int shoppingCartId);

        IPagedList<Order> GetAllOrders(string search, int? pageIndex, int? pageSize);

        IPagedList<T> GetAllOrdersWithCustomer<T>(string search, int? pageIndex, int? pageSize, Func<Order, Customer, T> func);

        void Create(Order order);

        void Update(Order order);

        void Save(Order order);

        void Delete(Order order);
    }
}
