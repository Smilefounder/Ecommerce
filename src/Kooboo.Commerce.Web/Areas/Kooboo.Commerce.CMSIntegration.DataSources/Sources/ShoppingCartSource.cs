using Kooboo.CMS.Common.Runtime.Dependency;
using Kooboo.Commerce.Api.Carts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Kooboo.CMS.Sites.Membership;

namespace Kooboo.Commerce.CMSIntegration.DataSources.Sources
{
    public class ShoppingCartSource : ApiCommerceSource
    {
        public ShoppingCartSource()
            : base("ShoppingCarts", typeof(IShoppingCartQuery), typeof(ShoppingCart))
        {
            InternalIncludablePaths.Remove("AppliedPromotions");
            InternalFilters.Add(new SourceFilterDefinition("ByCurrentCustomer"));
        }

        protected override object GetQuery(Api.ICommerceApi api)
        {
            return api.ShoppingCarts;
        }

        protected override void ApplyFilters(object query, List<SourceFilter> filters, CommerceSourceContext context)
        {
            var byCurrentCustomer = filters.Find(f => f.Name == "ByCurrentCustomer");
            if (byCurrentCustomer != null)
            {
                filters.Remove(byCurrentCustomer);

                var httpContext = new HttpContextWrapper(HttpContext.Current);
                var member = httpContext.Membership().GetMembershipUser();
                if (member != null && !String.IsNullOrWhiteSpace(member.UUID))
                {
                    filters.Add(new SourceFilter("ByAccountId")
                    {
                        ParameterValues = new Dictionary<string, object>
                        {
                            { "accountId", member.UUID }
                        }
                    });
                }
                else
                {
                    // TODO: What if frontend dev use a different cart session generation mechanism?
                    filters.Add(new SourceFilter("BySessionId")
                    {
                        ParameterValues = new Dictionary<string, object>
                        {
                            { "sessionId", httpContext.Session.SessionID }
                        }
                    });
                }
            }

            base.ApplyFilters(query, filters, context);
        }
    }
}