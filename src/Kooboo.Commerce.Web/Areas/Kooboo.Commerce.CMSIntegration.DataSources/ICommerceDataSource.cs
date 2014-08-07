using Kooboo.CMS.Sites.DataSource;
using Kooboo.CMS.Sites.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Kooboo.Commerce.CMSIntegration.DataSources
{
    public class CommerceDataSourceContext
    {
        public Site Site { get; set; }

        public Page Page { get; set; }

        public string Instance { get; set; }

        public IValueProvider ValueProvider { get; set; }

        public static CommerceDataSourceContext CreateFrom(DataSourceContext context)
        {
            var result = new CommerceDataSourceContext
            {
                Site = context.Site,
                Page = context.Page,
                Instance = context.Site.GetCommerceInstanceName(),
                ValueProvider = context.ValueProvider
            };

            return result;
        }
    }

    public interface ICommerceDataSource
    {
        string Name { get; }

        string EditorVirtualPath { get; }

        object Execute(CommerceDataSourceContext context);

        IDictionary<string, object> GetDefinitions(CommerceDataSourceContext context);

        IEnumerable<string> GetParameters();

        bool IsEnumerable();
    }
}