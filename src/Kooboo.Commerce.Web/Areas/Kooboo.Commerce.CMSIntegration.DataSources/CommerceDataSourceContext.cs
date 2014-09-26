using Kooboo.CMS.Membership.Models;
using Kooboo.CMS.Sites.DataRule;
using Kooboo.CMS.Sites.DataSource;
using Kooboo.CMS.Sites.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Kooboo.CMS.Sites.Membership;

namespace Kooboo.Commerce.CMSIntegration.DataSources
{
    public class CommerceDataSourceContext
    {
        public HttpContextBase HttpContext { get; set; }

        public Site Site { get; set; }

        public Page Page { get; set; }

        public string Instance { get; set; }

        public IValueProvider ValueProvider { get; set; }

        public static CommerceDataSourceContext CreateFrom(DataSourceContext context, HttpContextBase httpContext)
        {
            var result = new CommerceDataSourceContext
            {
                HttpContext = httpContext,
                Site = context.Site,
                Page = context.Page,
                Instance = context.Site.GetCommerceInstanceName(),
                ValueProvider = context.ValueProvider
            };

            return result;
        }

        public string GetFieldValue(string field)
        {
            return ParameterizedFieldValue.GetFieldValue(field, ValueProvider);
        }

        public object ResolveFieldValue(string field, Type type)
        {
            var fieldValue = GetFieldValue(field);
            if (String.IsNullOrWhiteSpace(fieldValue))
            {
                return null;
            }

            return StringConvert.ToObject(fieldValue, type);
        }

        public T ResolveFieldValue<T>(string field, T defaultValue)
        {
            var value = ResolveFieldValue(field, typeof(T));
            if (value == null)
            {
                return defaultValue;
            }

            return (T)value;
        }
    }
}