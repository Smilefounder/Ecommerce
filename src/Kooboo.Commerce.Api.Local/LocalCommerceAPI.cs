using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using Kooboo.Commerce.Data;
using Kooboo.CMS.Common.Runtime.Dependency;
using Kooboo.CMS.Common.Runtime;
using Kooboo.Commerce.Api;
using Kooboo.Commerce.API.Locations;
using Kooboo.Commerce.API.LocalProvider.Locations;
using Kooboo.Commerce.API.Brands;
using Kooboo.Commerce.API.LocalProvider.Brands;
using Kooboo.Commerce.API.Categories;
using Kooboo.Commerce.API.LocalProvider.Categories;
using Kooboo.Commerce.API.LocalProvider.Customers;
using Kooboo.Commerce.Api.Local;
using Kooboo.Commerce.API.LocalProvider.Products;
using Kooboo.Commerce.API.LocalProvider.ShoppingCarts;
using Kooboo.Commerce.API.LocalProvider.Orders;
using Kooboo.Commerce.API.LocalProvider.Payments;
using Kooboo.Commerce.Payments;
using Kooboo.Commerce.API.LocalProvider.Shipping;
using Kooboo.Commerce.API.Customers;
using Kooboo.Commerce.Api.Products;
using Kooboo.Commerce.API.Carts;
using Kooboo.Commerce.API.Orders;
using Kooboo.Commerce.API.Payments;
using Kooboo.Commerce.API.Shipping;

namespace Kooboo.Commerce.API.LocalProvider
{
    /// <summary>
    /// local commerce api
    /// this api uses the Kooboo.Commerce dll directly.
    /// </summary>
    [Dependency(typeof(ICommerceAPI), Key = "Local")]
    public class LocalCommerceAPI : ICommerceAPI
    {
        private LocalApiContext _context;

        public void Initialize(Api.ApiContext context)
        {
            // TODO: Not good! But without this, we are not able to resolve services.
            //       So we might need to think about improving the design of service apis. No idea for now.
            HttpContext.Current.Items["instance"] = context.Instance;
            HttpContext.Current.Items["language"] = context.Culture.Name;

            _context = new LocalApiContext(context, CommerceInstance.Current.Database, GetServiceFactory());
        }

        public ICountryAPI Countries
        {
            get
            {
                return new CountryAPI(_context);
            }
        }

        public IBrandAPI Brands
        {
            get
            {
                return new BrandAPI(GetServiceFactory().Brands);
            }
        }

        public ICategoryAPI Categories
        {
            get
            {
                return new CategoryAPI(GetServiceFactory().Categories);
            }
        }

        public ICustomerAPI Customers
        {
            get
            {
                return new CustomerAPI(_context);
            }
        }

        public IProductApi Products
        {
            get
            {
                return new ProductApi(_context);
            }
        }

        public IShoppingCartAPI ShoppingCarts
        {
            get
            {
                return new ShoppingCartAPI(_context, Customers);
            }
        }

        public IOrderAPI Orders
        {
            get
            {
                return new OrderAPI(_context);
            }
        }

        public IPaymentAPI Payments
        {
            get
            {
                return new LocalPaymentAPI(_context, EngineContext.Current.Resolve<IPaymentProcessorProvider>());
            }
        }

        public IPaymentMethodAPI PaymentMethods
        {
            get
            {
                return new LocalPaymentMethodAPI(_context.ServiceFactory.PaymentMethods, EngineContext.Current.Resolve<IPaymentProcessorProvider>());
            }
        }

        public IShippingMethodAPI ShippingMethods
        {
            get
            {
                return new LocalShippingMethodAPI(_context.ServiceFactory.ShippingMethods);
            }
        }

        private IServiceFactory GetServiceFactory()
        {
            return EngineContext.Current.Resolve<IServiceFactory>();
        }
    }
}
