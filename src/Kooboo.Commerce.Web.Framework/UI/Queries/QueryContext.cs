using Kooboo.Commerce.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.Web.Framework.Queries
{
    public class QueryContext
    {
        public CommerceInstance Instance { get; private set; }

        public string Keywords { get; private set; }

        public int PageIndex { get; private set; }

        public int PageSize { get; private set; }

        public object Config { get; private set; }

        public QueryContext(CommerceInstance instance, string keywords, int pageIndex, int pageSize, object config)
        {
            Instance = instance;
            Keywords = keywords;
            PageIndex = pageIndex;
            PageSize = pageSize;
            Config = config;
        }
    }
}
