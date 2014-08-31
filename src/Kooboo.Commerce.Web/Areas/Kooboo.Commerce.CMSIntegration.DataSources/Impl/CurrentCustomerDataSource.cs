using Kooboo.Commerce.CMSIntegration.DataSources.Generic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Kooboo.Commerce.Api;
using Kooboo.Commerce.Api.Customers;
using Kooboo.Commerce.Api.Metadata;

namespace Kooboo.Commerce.CMSIntegration.DataSources.Impl
{
    public class CurrentCustomerDataSource : GenericCommerceDataSource
    {
        public override string Name
        {
            get { return "CurrentCustomer"; }
        }

        public override IEnumerable<string> OptionalIncludeFields
        {
            get
            {
                return QueryDescriptors.Get(typeof(Query<Customer>)).OptionalIncludeFields;
            }
        }

        protected override object DoExecute(CommerceDataSourceContext context, ParsedGenericCommerceDataSourceSettings settings)
        {
            var user = context.HttpContext.GetMembershipUser();
            if (user == null)
            {
                return null;
            }

            var query = context.Site.Commerce().Customers.Query().ByAccountId(user.UUID);

            if (settings.Includes != null)
            {
                foreach (var include in settings.Includes)
                {
                    query.Include(include);
                }
            }

            return query.FirstOrDefault();
        }
    }
}