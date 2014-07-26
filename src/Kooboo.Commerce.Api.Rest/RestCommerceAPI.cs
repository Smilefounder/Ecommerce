using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Kooboo.CMS.Common.Runtime.Dependency;
using Kooboo.CMS.Common.Runtime;

namespace Kooboo.Commerce.API.RestProvider
{
    /// <summary>
    /// commerce api by using restful web api
    /// </summary>
    [Dependency(typeof(ICommerceAPI), Key = "Rest")]
    public class RestCommerceAPI : ICommerceAPI
    {
        private string _instance;
        private string _language;
        private string _webApiHost;

        // TODO: Fix this when I get time

        /// <summary>
        /// set the commerce instance and language to fields.
        /// the commerce instance and language info will be passed to remote web api by query string
        /// </summary>
        /// <param name="instance">commerce instance name</param>
        /// <param name="language">language</param>
        /// <param name="currency">currency</param>
        /// <param name="settings">kooboo cms site's custom field settings</param>
        //public override void InitCommerceInstance(string instance, string language, string currency, Dictionary<string, string> settings)
        //{
        //    this._instance = instance;
        //    this._language = language;
        //    if (settings != null && settings.ContainsKey("WebAPIHost"))
        //        this._webApiHost = settings["WebAPIHost"].TrimEnd('/');
        //    if (string.IsNullOrEmpty(_webApiHost))
        //        throw new Exception("No web api host.");
        //}

        ///// <summary>
        ///// get the corresponding api from runtime context
        ///// </summary>
        ///// <typeparam name="T">api interface</typeparam>
        ///// <returns>api</returns>
        //protected override T GetAPI<T>()
        //{
        //    if (string.IsNullOrEmpty(_webApiHost))
        //        throw new Exception("No web api host.");
        //    T api = EngineContext.Current.Resolve<T>();
        //    if (api is RestApiBase)
        //    {
        //        var restApi = api as RestApiBase;
        //        restApi.WebAPIHost = string.Join("/", _webApiHost, _instance);
        //    }
        //    return api;
        //}

        public void Initialize(Api.ApiContext context)
        {
            throw new NotImplementedException();
        }

        public API.Locations.ICountryAPI Countries
        {
            get { throw new NotImplementedException(); }
        }

        public API.Brands.IBrandAPI Brands
        {
            get { throw new NotImplementedException(); }
        }

        public API.Categories.ICategoryAPI Categories
        {
            get { throw new NotImplementedException(); }
        }

        public API.Customers.ICustomerAPI Customers
        {
            get { throw new NotImplementedException(); }
        }

        public Api.Products.IProductApi Products
        {
            get { throw new NotImplementedException(); }
        }

        public Carts.IShoppingCartAPI ShoppingCarts
        {
            get { throw new NotImplementedException(); }
        }

        public API.Orders.IOrderAPI Orders
        {
            get { throw new NotImplementedException(); }
        }

        public API.Payments.IPaymentAPI Payments
        {
            get { throw new NotImplementedException(); }
        }

        public API.Payments.IPaymentMethodAPI PaymentMethods
        {
            get { throw new NotImplementedException(); }
        }

        public Shipping.IShippingMethodAPI ShippingMethods
        {
            get { throw new NotImplementedException(); }
        }
    }
}
