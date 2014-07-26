using Kooboo.Commerce.Api.Brands;
using Kooboo.Commerce.Api.Categories;
using Kooboo.Commerce.Api.Customers;
using Kooboo.Commerce.Api.Orders;
using Kooboo.Commerce.Api.Payments;
using Kooboo.Commerce.Api.Shipping;
using Kooboo.Commerce.Api.Carts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Kooboo.Commerce.Api.Products;
using Kooboo.Commerce.Api;
using Kooboo.Commerce.Api.Countries;

namespace Kooboo.Commerce.Api
{
    /// <summary>
    /// commerce api
    /// </summary>
    public interface ICommerceApi
    {
        void Initialize(ApiContext context);

        ICountryApi Countries { get; }

        IBrandApi Brands { get; }

        ICategoryApi Categories { get; }

        ICustomerApi Customers { get; }

        IProductApi Products { get; }

        IShoppingCartApi ShoppingCarts { get; }

        IOrderApi Orders { get; }

        IPaymentApi Payments { get; }

        IPaymentMethodApi PaymentMethods { get; }

        IShippingMethodApi ShippingMethods { get; }
    }
}
