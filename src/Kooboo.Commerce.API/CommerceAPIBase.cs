using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Kooboo.Commerce.API.Brands;
using Kooboo.Commerce.API.Brands.Services;
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
        public abstract void InitCommerceInstance(string instance, string language);

        protected virtual Q GetAPI<Q>() where Q : class
        {
            return EngineContext.Current.Resolve<Q>();
        }

        public ICountryQuery Countries
        {
            get { return GetAPI<ICountryQuery>(); }
        }

        public IBrandQuery Brands
        {
            get { return GetAPI<IBrandQuery>(); }
        }

        public ICategoryQuery Categories
        {
            get { return GetAPI<ICategoryQuery>(); }
        }

        public IPaymentMethodAccess PaymentMethods
        {
            get { return GetAPI<IPaymentMethodAccess>(); }
        }

        public ICustomerQuery Customers
        {
            get { return GetAPI<ICustomerQuery>(); }
        }

        public IProductQuery Products
        {
            get { return GetAPI<IProductQuery>(); }
        }

        public IShoppingCartQuery ShoppingCarts
        {
            get { return GetAPI<IShoppingCartQuery>(); }
        }

        public IOrderQuery Orders
        {
            get { return GetAPI<IOrderQuery>(); }
        }

        public IPaymentAccess Payments
        {
            get { return GetAPI<IPaymentAccess>(); }
        }
    }
}
