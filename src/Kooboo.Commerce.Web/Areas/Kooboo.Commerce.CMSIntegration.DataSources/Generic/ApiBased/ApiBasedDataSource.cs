using Kooboo.Commerce.Api;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Kooboo.CMS.Sites.Membership;
using System.Globalization;
using Kooboo.Commerce.Api.Metadata;
using System.Runtime.Serialization;

namespace Kooboo.Commerce.CMSIntegration.DataSources.Generic.ApiBased
{
    [DataContract]
    public abstract class ApiBasedDataSource<T> : GenericCommerceDataSource
        where T : class
    {
        public override IEnumerable<FilterDescription> Filters
        {
            get
            {
                return GetQueryDescriptor().Filters;
            }
        }

        public override IEnumerable<string> SortFields
        {
            get { return GetQueryDescriptor().SortFields; }
        }

        public override IEnumerable<string> OptionalIncludeFields
        {
            get { return GetQueryDescriptor().OptionalIncludeFields; }
        }

        protected override object DoExecute(CommerceDataSourceContext context, ParsedGenericCommerceDataSourceSettings settings)
        {
            var user = new HttpContextWrapper(HttpContext.Current).Membership().GetMembershipUser();
            var accountId = user == null ? null : user.UUID;

            var apiContext = new ApiContext(context.Site.GetCommerceInstanceName(), CultureInfo.GetCultureInfo(context.Site.Culture), context.Site.GetCurrency(), accountId);
            var api = ApiService.Get(context.Site.ApiType(), apiContext);

            var query = Query(api);

            // Filters
            if (settings.Filters != null && settings.Filters.Count > 0)
            {
                foreach (var filter in settings.Filters)
                {
                    query.Filters.Add(new QueryFilter(filter.Name, filter.ParameterValues));
                }
            }

            // Sorting
            if (!String.IsNullOrEmpty(settings.SortField))
            {
                // TODO: Fix sorting
                query.Sorts.Add(new Sort(settings.SortField, Kooboo.Commerce.Api.SortDirection.Asc));
            }

            if (settings.TakeOperation == TakeOperation.First)
            {
                return query.FirstOrDefault();
            }

            if (settings.EnablePaging)
            {
                var pageSize = settings.PageSize.GetValueOrDefault(50);
                var pageNumber = settings.PageNumber.GetValueOrDefault(1);

                query.Skip((pageNumber - 1) * pageSize).Take(pageSize);
            }
            else
            {
                if (settings.Top != null && settings.Top.Value > 0)
                {
                    query.Take(settings.Top.Value);
                }
            }

            return query.ToList();
        }

        public override IDictionary<string, object> GetDefinitions(CommerceDataSourceContext context)
        {
            return DataSourceDefinitionHelper.GetDefinitions(typeof(T));
        }

        protected abstract Query<T> Query(ICommerceApi api);

        private IQueryDescriptor GetQueryDescriptor()
        {
            return QueryDescriptors.Get(typeof(Query<T>));
        }
    }
}