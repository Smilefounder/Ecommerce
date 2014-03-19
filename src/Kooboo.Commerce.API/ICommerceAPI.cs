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

        ICountryQuery Country { get; }
        IBrandQuery Brands { get; }
        ICategoryQuery Category { get; }
        //IPaymentMethodQuery PaymentMethod { get; }
        ICustomerQuery Customer { get; }
        IProductQuery Product { get; }
        IShoppingCartQuery ShoppingCart { get; }
        IOrderQuery Order { get; }
        IPaymentAPI Payment { get; }
    }
}
