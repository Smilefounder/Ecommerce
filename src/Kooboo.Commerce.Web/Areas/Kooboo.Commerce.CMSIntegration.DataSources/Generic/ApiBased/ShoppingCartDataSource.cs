using Kooboo.CMS.Common.Runtime.Dependency;
using Kooboo.Commerce.Api.Carts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Kooboo.CMS.Sites.Membership;
using System.Runtime.Serialization;

namespace Kooboo.Commerce.CMSIntegration.DataSources.Generic.ApiBased
{
    [DataContract]
    [KnownType(typeof(ShoppingCartDataSource))]
    public class ShoppingCartDataSource : ApiBasedDataSource
    {
        public override string Name
        {
            get { return "ShoppingCarts"; }
        }

        protected override Type QueryType
        {
            get { return typeof(IShoppingCartQuery); }
        }

        protected override Type ItemType
        {
            get { return typeof(ShoppingCart); }
        }

        public ShoppingCartDataSource()
        {
            OnDeserialized(default(StreamingContext));
        }

        [OnDeserialized]
        private void OnDeserialized(StreamingContext context)
        {
            InternalIncludablePaths.Remove("AppliedPromotions");
            InternalFilters.Add(new FilterDefinition("ByCurrentCustomer"));
        }

        protected override object GetQuery(Api.ICommerceApi api)
        {
            return api.ShoppingCarts;
        }

        protected override void ApplyFilters(object query, List<ParsedFilter> filters, CommerceDataSourceContext context)
        {
            var byCurrentCustomer = filters.Find(f => f.Name == "ByCurrentCustomer");
            if (byCurrentCustomer != null)
            {
                filters.Remove(byCurrentCustomer);

                var httpContext = new HttpContextWrapper(HttpContext.Current);
                var member = httpContext.Membership().GetMembershipUser();
                if (member != null && !String.IsNullOrWhiteSpace(member.UUID))
                {
                    filters.Add(new ParsedFilter("ByAccountId")
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
                    filters.Add(new ParsedFilter("BySessionId")
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