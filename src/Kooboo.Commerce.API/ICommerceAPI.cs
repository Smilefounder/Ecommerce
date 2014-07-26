using Kooboo.Commerce.API.Brands;
using Kooboo.Commerce.API.Categories;
using Kooboo.Commerce.API.Customers;
using Kooboo.Commerce.API.Locations;
using Kooboo.Commerce.API.Orders;
using Kooboo.Commerce.API.Payments;
using Kooboo.Commerce.API.Shipping;
using Kooboo.Commerce.API.Carts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Kooboo.Commerce.Api.Products;
using Kooboo.Commerce.Api;

namespace Kooboo.Commerce.API
{
    /// <summary>
    /// commerce api
    /// </summary>
    public interface ICommerceAPI
    {
        void Initialize(ApiContext context);

        /// <summary>
        /// country api
        /// </summary>
        ICountryAPI Countries { get; }
        /// <summary>
        /// brand api
        /// </summary>
        IBrandAPI Brands { get; }
        /// <summary>
        /// category api
        /// </summary>
        ICategoryAPI Categories { get; }
        /// <summary>
        /// customer api
        /// </summary>
        ICustomerAPI Customers { get; }
        /// <summary>
        /// product api
        /// </summary>
        IProductApi Products { get; }
        /// <summary>
        /// shopping cart api
        /// </summary>
        IShoppingCartAPI ShoppingCarts { get; }
        /// <summary>
        /// order api
        /// </summary>
        IOrderAPI Orders { get; }
        /// <summary>
        /// payment api
        /// </summary>
        IPaymentAPI Payments { get; }
        /// <summary>
        /// payment method api
        /// </summary>
        IPaymentMethodAPI PaymentMethods { get; }

        IShippingMethodAPI ShippingMethods { get; }
    }
}
