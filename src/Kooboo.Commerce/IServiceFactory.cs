using Kooboo.Commerce.Brands.Services;
using Kooboo.Commerce.Carts.Services;
using Kooboo.Commerce.Categories.Services;
using Kooboo.Commerce.Customers.Services;
using Kooboo.Commerce.EAV.Services;
using Kooboo.Commerce.Locations.Services;
using Kooboo.Commerce.Orders.Services;
using Kooboo.Commerce.Payments.Services;
using Kooboo.Commerce.Products.Services;
using Kooboo.Commerce.Promotions.Services;
using Kooboo.Commerce.Shipping.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce
{
    public interface IServiceFactory
    {
        ICountryService Countries { get; }

        IBrandService Brands { get; }

        ICategoryService Categories { get; }

        IProductTypeService ProductTypes { get; }

        ICustomFieldService CustomFields { get; }

        IProductService Products { get; }

        ICustomerService Customers { get; }

        IShoppingCartService Carts { get; }

        IOrderService Orders { get; }

        IPaymentMethodService PaymentMethods { get; }

        IPaymentService Payments { get; }

        IPromotionService Promotions { get; }

        IShippingMethodService ShippingMethods { get; }
    }
}
