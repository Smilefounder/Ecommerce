using Kooboo.CMS.Sites.DataSource;
using Kooboo.CMS.Sites.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Kooboo.Commerce.CMSIntegration.DataSources.Sources
{
    public class CommerceSourceContext
    {
        public DataSourceContext DataSourceContext { get; private set; }

        public Site Site
        {
            get
            {
                return DataSourceContext.Site;
            }
        }

        public QueryType QueryType { get; set; }

        public List<SourceFilter> Filters { get; set; }

        public List<string> Includes { get; set; }

        public string SortField { get; set; }

        public SortDirection SortDirection { get; set; }

        public int? Top { get; set; }

        public bool EnablePaging { get; set; }

        public int? PageSize { get; set; }

        public int? PageNumber { get; set; }

        public CommerceSourceContext(DataSourceContext dataSourceContext)
        {
            DataSourceContext = dataSourceContext;
            Filters = new List<SourceFilter>();
            Includes = new List<string>();
        }
    }
}