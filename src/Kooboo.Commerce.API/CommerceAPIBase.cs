using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Kooboo.Commerce.API.Brands;
using Kooboo.Commerce.API.Locations;
using Kooboo.Commerce.API.Categories;
using Kooboo.Commerce.API.Customers;
using Kooboo.Commerce.API.Products;
using Kooboo.Commerce.API.Orders;
using Kooboo.Commerce.API.ShoppingCarts;
using Kooboo.CMS.Common.Runtime;
using Kooboo.CMS.Common.Runtime.Dependency;
using Kooboo.Commerce.API.Payments;

namespace Kooboo.Commerce.API
{
    public abstract class CommerceAPIBase : ICommerceAPI
    {
        public abstract void InitCommerceInstance(string instance, string language, Dictionary<string, string> settings);

        protected virtual Q GetAPI<Q>() where Q : class
        {
            return EngineContext.Current.Resolve<Q>();
        }

        public ICountryAPI Countries
        {
            get { return GetAPI<ICountryAPI>(); }
        }

        public IBrandAPI Brands
        {
            get { return GetAPI<IBrandAPI>(); }
        }

        public ICategoryAPI Categories
        {
            get { return GetAPI<ICategoryAPI>(); }
        }

        public IPaymentMethodAPI PaymentMethods
        {
            get { return GetAPI<IPaymentMethodAPI>(); }
        }

        public ICustomerAPI Customers
        {
            get { return GetAPI<ICustomerAPI>(); }
        }

        public IProductAPI Products
        {
            get { return GetAPI<IProductAPI>(); }
        }

        public IShoppingCartAPI ShoppingCarts
        {
            get { return GetAPI<IShoppingCartAPI>(); }
        }

        public IOrderAPI Orders
        {
            get { return GetAPI<IOrderAPI>(); }
        }

        public IPaymentAPI Payments
        {
            get { return GetAPI<IPaymentAPI>(); }
        }
    }
}
