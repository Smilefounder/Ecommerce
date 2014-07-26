using Kooboo.CMS.Common.Runtime;
using Kooboo.CMS.Common.Runtime.Dependency;
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
    [Dependency(typeof(IServiceFactory))]
    public class DefaultServiceFactory : IServiceFactory
    {
        public ICountryService Countries
        {
            get { return Resolve<ICountryService>(); }
        }

        public IBrandService Brands
        {
            get { return Resolve<IBrandService>(); }
        }

        public ICategoryService Categories
        {
            get { return Resolve<ICategoryService>(); }
        }

        public ICustomerService Customers
        {
            get { return Resolve<ICustomerService>(); }
        }

        public IProductTypeService ProductTypes
        {
            get { return Resolve<IProductTypeService>(); }
        }

        public ICustomFieldService CustomFields
        {
            get { return Resolve<ICustomFieldService>(); }
        }

        public IProductService Products
        {
            get { return Resolve<IProductService>(); }
        }

        public IShoppingCartService Carts
        {
            get { return Resolve<IShoppingCartService>(); }
        }

        public IOrderService Orders
        {
            get { return Resolve<IOrderService>(); }
        }

        public IPaymentMethodService PaymentMethods
        {
            get { return Resolve<IPaymentMethodService>(); }
        }

        public IPaymentService Payments
        {
            get { return Resolve<IPaymentService>(); }
        }

        public IPromotionService Promotions
        {
            get { return Resolve<IPromotionService>(); }
        }

        public IShippingMethodService ShippingMethods
        {
            get { return Resolve<IShippingMethodService>(); }
        }

        private T Resolve<T>() where T : class
        {
            return EngineContext.Current.Resolve<T>();
        }
    }
}
