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

        public ICountryQuery Country
        {
            get { return GetAPI<ICountryQuery>(); }
        }

        public IBrandQuery Brands
        {
            get { return GetAPI<IBrandQuery>(); }
        }

        public ICategoryQuery Category
        {
            get { return GetAPI<ICategoryQuery>(); }
        }

        //public IPaymentMethodAPI PaymentMethod
        //{
        //    get { return GetAPI<IPaymentMethodAPI>(); }
        //}

        public ICustomerQuery Customer
        {
            get { return GetAPI<ICustomerQuery>(); }
        }

        public IProductQuery Product
        {
            get { return GetAPI<IProductQuery>(); }
        }

        public IShoppingCartQuery ShoppingCart
        {
            get { return GetAPI<IShoppingCartQuery>(); }
        }

        public IOrderQuery Order
        {
            get { return GetAPI<IOrderQuery>(); }
        }

        public IPaymentAPI Payment
        {
            get { return GetAPI<IPaymentAPI>(); }
        }
    }
}
