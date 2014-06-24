using Kooboo.Commerce.API.Brands;
using Kooboo.Commerce.API.Categories;
using Kooboo.Commerce.API.Customers;
using Kooboo.Commerce.API.Locations;
using Kooboo.Commerce.API.Orders;
using Kooboo.Commerce.API.Payments;
using Kooboo.Commerce.API.Pricing;
using Kooboo.Commerce.API.Products;
using Kooboo.Commerce.API.Shipping;
using Kooboo.Commerce.API.ShoppingCarts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.API
{
    /// <summary>
    /// commerce api
    /// </summary>
    public interface ICommerceAPI
    {
        /// <summary>
        /// all commerce services work on a specified commerce instance and language.
        /// set the instance and language in the runtime context before calling commerce services.
        /// </summary>
        /// <param name="instance">commerce intance</param>
        /// <param name="language">lanuage</param>
        /// <param name="currency">currency</param>
        /// <param name="settings">the extra settings for provider</param>
        void InitCommerceInstance(string instance, string language, string currency, Dictionary<string, string> settings);

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
        IProductAPI Products { get; }
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
        /// <summary>
        /// Api for calculating order prices.
        /// </summary>
        IPriceAPI Prices { get; }
    }
}
