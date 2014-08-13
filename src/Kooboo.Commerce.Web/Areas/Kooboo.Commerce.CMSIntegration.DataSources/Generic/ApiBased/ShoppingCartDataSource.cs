using Kooboo.CMS.Common.Runtime.Dependency;
using Kooboo.Commerce.Api.Carts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Kooboo.CMS.Sites.Membership;
using System.Runtime.Serialization;
using Kooboo.Commerce.Api;

namespace Kooboo.Commerce.CMSIntegration.DataSources.Generic.ApiBased
{
    [DataContract]
    [KnownType(typeof(ShoppingCartDataSource))]
    public class ShoppingCartDataSource : ApiDataSource<ShoppingCart>
    {
        public override string Name
        {
            get { return "ShoppingCarts"; }
        }

        protected override Query<ShoppingCart> Query(ICommerceApi api)
        {
            return api.ShoppingCarts.Query();
        }
    }
}