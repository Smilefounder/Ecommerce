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

namespace Kooboo.Commerce.CMSIntegration.DataSources.Impl
{
    [DataContract]
    [KnownType(typeof(ShoppingCartDataSource))]
    public class ShoppingCartDataSource : ApiQueryBasedDataSource<ShoppingCart>
    {
        public override string Name
        {
            get { return "ShoppingCarts"; }
        }

        public override IEnumerable<Api.Metadata.FilterDescription> Filters
        {
            get
            {
                // ByCurrentCustomer is a responsiblity of data source, not api, so we add this filter here instead in the api
                var filters = base.Filters.ToList();
                filters.Add(new Api.Metadata.FilterDescription("ByCurrentCustomer"));
                return filters;
            }
        }

        protected override Query<ShoppingCart> ApplyFilters(Query<ShoppingCart> query, IList<ParsedFilter> filters, CommerceDataSourceContext context)
        {
            if (filters != null)
            {
                // Translate ByCurrentCustomer filter to underlying filters
                var filter = filters.FirstOrDefault(f => f.Name == "ByCurrentCustomer");
                if (filter != null)
                {
                    filters.Remove(filter);

                    var user = context.HttpContext.GetMembershipUser();
                    if (user != null)
                    {
                        query = query.ByCustomerEmail(user.Email);
                    }
                    else
                    {
                        var sessionId = EngineContext.Current.Resolve<IShoppingCartSessionIdProvider>().GetCurrentSessionId(true);
                        query = query.BySessionId(sessionId);
                    }
                }
            }

            return base.ApplyFilters(query, filters, context);
        }

        protected override Query<ShoppingCart> Query(ICommerceApi api)
        {
            return api.ShoppingCarts.Query();
        }
    }
}