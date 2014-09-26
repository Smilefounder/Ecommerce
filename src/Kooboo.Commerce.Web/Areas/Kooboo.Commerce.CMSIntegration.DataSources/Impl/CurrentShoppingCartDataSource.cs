using Kooboo.CMS.Common.Runtime.Dependency;
using Kooboo.Commerce.Api.Carts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Kooboo.CMS.Sites.Membership;
using System.Runtime.Serialization;
using Kooboo.Commerce.Api;
using Kooboo.Commerce.CMSIntegration.DataSources.Generic;
using Kooboo.CMS.Common.Runtime;
using Kooboo.Commerce.Api.Metadata;

namespace Kooboo.Commerce.CMSIntegration.DataSources.Impl
{
    [DataContract]
    [KnownType(typeof(CurrentShoppingCartDataSource))]
    public class CurrentShoppingCartDataSource : GenericCommerceDataSource
    {
        public override string Name
        {
            get { return "CurrentShoppingCart"; }
        }

        public override IEnumerable<string> OptionalIncludeFields
        {
            get
            {
                return QueryDescriptors.Get(typeof(Query<ShoppingCart>)).OptionalIncludeFields;
            }
        }

        protected override object DoExecute(CommerceDataSourceContext context, ParsedGenericCommerceDataSourceSettings settings)
        {
            var cartId = 0;
            var user = context.HttpContext.GetMembershipUser();
            if (user == null)
            {
                var sessionIdProvider = EngineContext.Current.Resolve<ICartSessionIdProvider>();
                cartId = context.Site.Commerce().ShoppingCarts.GetCartIdBySessionId(sessionIdProvider.GetCurrentSessionId(true));
            }
            else
            {
                cartId = context.Site.Commerce().ShoppingCarts.GetCartIdByCustomer(user.Email);
            }

            var query = context.Site.Commerce().ShoppingCarts.Query().ById(cartId);

            if (settings.Includes != null && settings.Includes.Count > 0)
            {
                foreach (var path in settings.Includes)
                {
                    query.Include(path);
                }
            }

            return query.FirstOrDefault();
        }
    }
}