using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Kooboo.CMS.Common.Runtime.Dependency;
using Kooboo.CMS.Common.Runtime;

namespace Kooboo.Commerce.API.RestProvider
{
    [Dependency(typeof(ICommerceAPI), ComponentLifeStyle.InRequestScope)]
    public class RestCommerceAPI : CommerceAPIBase
    {
        private string _instance;
        private string _language;
        private string _webApiHost;

        public override void InitCommerceInstance(string instance, string language, Dictionary<string, string> settings)
        {
            this._instance = instance;
            this._language = language;
            if (settings != null && settings.ContainsKey("WebAPIHost"))
                this._webApiHost = settings["WebAPIHost"].TrimEnd('/');
            if (string.IsNullOrEmpty(_webApiHost))
                throw new Exception("No web api host.");
        }

        protected override T GetAPI<T>()
        {
            if (string.IsNullOrEmpty(_webApiHost))
                throw new Exception("No web api host.");
            T api = EngineContext.Current.Resolve<T>();
            if (api is RestApiBase)
            {
                var restApi = api as RestApiBase;
                restApi.WebAPIHost = string.Join("/", _webApiHost, _instance);
            }
            return api;
        }
    }
}
