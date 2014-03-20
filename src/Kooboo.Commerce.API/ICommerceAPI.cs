using Kooboo.Commerce.API.Brands.Services;
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

        ICountryQuery Countries { get; }
        IBrandQuery Brands { get; }
        ICategoryQuery Categories { get; }
        IPaymentMethodQuery PaymentMethods { get; }
        ICustomerQuery Customers { get; }
        IProductQuery Products { get; }
        IShoppingCartQuery ShoppingCarts { get; }
        IOrderQuery Orders { get; }
        IPaymentAPI Payments { get; }
    }
}
