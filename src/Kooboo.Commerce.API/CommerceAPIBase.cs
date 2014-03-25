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
    /// <summary>
    /// commerce api base
    /// </summary>
    public abstract class CommerceAPIBase : ICommerceAPI
    {
        /// <summary>
        /// all commerce services work on a specified commerce instance and language.
        /// set the instance and language in the runtime context before calling commerce services.
        /// </summary>
        /// <param name="instance">commerce intance</param>
        /// <param name="language">lanuage</param>
        /// <param name="settings">the extra settings for provider</param>
        public abstract void InitCommerceInstance(string instance, string language, Dictionary<string, string> settings);
        /// <summary>
        /// get the corresponding api from runtime context
        /// </summary>
        /// <typeparam name="Q">api interface</typeparam>
        /// <returns>api</returns>
        protected virtual Q GetAPI<Q>() where Q : class
        {
            return EngineContext.Current.Resolve<Q>();
        }
        /// <summary>
        /// country api
        /// </summary>
        public ICountryAPI Countries
        {
            get { return GetAPI<ICountryAPI>(); }
        }
        /// <summary>
        /// brand api
        /// </summary>
        public IBrandAPI Brands
        {
            get { return GetAPI<IBrandAPI>(); }
        }
        /// <summary>
        /// category api
        /// </summary>
        public ICategoryAPI Categories
        {
            get { return GetAPI<ICategoryAPI>(); }
        }
        /// <summary>
        /// payment method api
        /// </summary>
        public IPaymentMethodAPI PaymentMethods
        {
            get { return GetAPI<IPaymentMethodAPI>(); }
        }
        /// <summary>
        /// customer api
        /// </summary>
        public ICustomerAPI Customers
        {
            get { return GetAPI<ICustomerAPI>(); }
        }
        /// <summary>
        /// product api
        /// </summary>
        public IProductAPI Products
        {
            get { return GetAPI<IProductAPI>(); }
        }
        /// <summary>
        /// shopping cart api
        /// </summary>
        public IShoppingCartAPI ShoppingCarts
        {
            get { return GetAPI<IShoppingCartAPI>(); }
        }
        /// <summary>
        /// order api
        /// </summary>
        public IOrderAPI Orders
        {
            get { return GetAPI<IOrderAPI>(); }
        }
        /// <summary>
        /// payment api
        /// </summary>
        public IPaymentAPI Payments
        {
            get { return GetAPI<IPaymentAPI>(); }
        }
    }
}
