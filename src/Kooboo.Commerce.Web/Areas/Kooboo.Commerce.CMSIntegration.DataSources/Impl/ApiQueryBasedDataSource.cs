using Kooboo.Commerce.Api;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Kooboo.CMS.Sites.Membership;
using System.Globalization;
using Kooboo.Commerce.Api.Metadata;
using System.Runtime.Serialization;
using Kooboo.Commerce.CMSIntegration.DataSources.Generic;

namespace Kooboo.Commerce.CMSIntegration.DataSources.Impl
{
    [DataContract]
    public abstract class ApiQueryBasedDataSource<T> : GenericCommerceDataSource
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
            var query = Query(context.Site.Commerce());

            // Filters
            query = ApplyFilters(query, settings.Filters, context);

            // Sorting
            if (!String.IsNullOrEmpty(settings.SortField))
            {
                query.Sorts.Add(new Sort(settings.SortField, settings.SortDirection));
            }

            if (settings.TakeOperation == TakeOperation.First)
            {
                return query.FirstOrDefault();
            }

            // Pagination
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

            // Includes
            if (settings.Includes != null && settings.Includes.Count > 0)
            {
                foreach (var path in settings.Includes)
                {
                    query.Include(path);
                }
            }

            return query.ToList();
        }

        protected virtual Query<T> ApplyFilters(Query<T> query, IList<ParsedFilter> filters, CommerceDataSourceContext context)
        {
            if (filters == null)
            {
                return query;
            }

            foreach (var filter in filters)
            {
                query.Filters.Add(new QueryFilter(filter.Name, filter.ParameterValues));
            }

            return query;
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