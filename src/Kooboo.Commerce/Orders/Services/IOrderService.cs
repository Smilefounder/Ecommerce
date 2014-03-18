﻿using System;
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

        //Order GetByShoppingCartId(int shoppingCartId);

        Order CreateOrderFromShoppingCart(ShoppingCart shoppingCart, MembershipUser user, bool expireShoppingCart);

        //IPagedList<Order> GetAllOrders(string search, int? pageIndex, int? pageSize);

        //IPagedList<T> GetAllOrdersWithCustomer<T>(string search, int? pageIndex, int? pageSize, Func<Order, Customer, T> func);

        //IPagedList<Order> GetAllCustomerOrders(int customerId, int? pageIndex, int? pageSize);

        void Create(Order order);

        void Update(Order order);

        void Save(Order order);

        void Delete(Order order);
    }
}
