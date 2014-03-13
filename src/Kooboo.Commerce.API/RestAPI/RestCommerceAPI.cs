using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Kooboo;
using Kooboo.Commerce.Brands;
using Kooboo.Commerce.Categories;
using Kooboo.Commerce.Customers;
using Kooboo.Commerce.EAV;
using Kooboo.Commerce.Events;
using Kooboo.Commerce.Locations;
using Kooboo.Commerce.Orders;
using Kooboo.Commerce.Payments;
using Kooboo.Commerce.Pricing;
using Kooboo.Commerce.Products;
using Kooboo.Commerce.Promotions;
using Kooboo.Commerce.ShoppingCarts;
using Kooboo.CMS.Common.Runtime.Dependency;
using Kooboo.CMS.Common.Runtime;

namespace Kooboo.Commerce.API.RestAPI
{
    [Dependency(typeof(ICommerceAPI), ComponentLifeStyle.InRequestScope, Key = "RestCommerceAPI")]
    public class RestCommerceAPI : CommerceAPIBase
    {
        private string _instance;
        private string _language;
        private string _webApiHost;

        public override void InitCommerceInstance(string instance, string language)
        {
            this._instance = instance;
            this._language = language;
        }

        protected override T GetAPI<T>()
        {
            if (string.IsNullOrEmpty(_webApiHost))
                throw new Exception("No web api host.");
            T api = EngineContext.Current.Resolve<T>("RestAPI");
            if (api is RestApiBase)
            {
                var restApi = api as RestApiBase;
                restApi.WebAPIHost = string.Join("/", _webApiHost, _instance);
            }
            return api;
        }

        public void SetWebAPIHost(string webApiHost)
        {
            _webApiHost = webApiHost;
            if (!string.IsNullOrEmpty(_webApiHost) && _webApiHost.EndsWith("/"))
                _webApiHost = _webApiHost.TrimEnd('/');
        }
    }
}
