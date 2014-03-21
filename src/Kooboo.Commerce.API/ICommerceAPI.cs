﻿using Kooboo.Commerce.API.Brands;
using Kooboo.Commerce.API.Categories;
using Kooboo.Commerce.API.Customers;
using Kooboo.Commerce.API.Locations;
using Kooboo.Commerce.API.Orders;
using Kooboo.Commerce.API.Payments;
using Kooboo.Commerce.API.Products;
using Kooboo.Commerce.API.ShoppingCarts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.API
{
    public interface ICommerceAPI
    {
        void InitCommerceInstance(string instance, string language);

        ICountryAPI Countries { get; }
        IBrandAPI Brands { get; }
        ICategoryAPI Categories { get; }
        ICustomerAPI Customers { get; }
        IProductAPI Products { get; }
        IShoppingCartAPI ShoppingCarts { get; }
        IOrderAPI Orders { get; }
        IPaymentAPI Payments { get; }
        IPaymentMethodAPI PaymentMethods { get; }
    }
}
