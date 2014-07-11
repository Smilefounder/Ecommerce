using Kooboo.Commerce.Web.Framework.Queries;
using Kooboo.Web.Mvc.Paging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Kooboo.Commerce.Web.Areas.Commerce.Models.Queries
{
    public class QueryGridModel
    {
        public QueryInfo CurrentQueryInfo { get; set; }

        public IList<QueryInfo> AllQueryInfos { get; set; }

        public IPagedList CurrentQueryResult { get; set; }
    }
}