using Kooboo.CMS.Common.Runtime;
using Kooboo.Commerce.API.Accessories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.API
{
    public static class CommerceAPIExtensions
    {
        // TODO: Need this extension method?
        //       If yes, then change all properties in ICommerceAPI to method calls to keep consistent?
        //       Because now it's like this:
        //          var products = api.Products.ById...
        //          var accessories = api.Accessories().ForProduct(...) 
        public static IAccessoryAPI Accessories(this ICommerceAPI api)
        {
            return EngineContext.Current.Resolve<IAccessoryAPI>();
        }
    }
}
