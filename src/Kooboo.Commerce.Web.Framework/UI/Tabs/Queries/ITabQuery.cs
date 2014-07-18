using Kooboo.CMS.Common.Runtime;
using Kooboo.Commerce.Data;
using Kooboo.Commerce.Locations;
using Kooboo.Commerce.Web.Framework.UI;
using Kooboo.Web.Mvc.Paging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;

namespace Kooboo.Commerce.Web.Framework.UI.Tabs.Queries
{
    public interface ITabQuery
    {
        string Name { get; }

        string DisplayName { get; }

        Type ConfigType { get; }

        Type ResultType { get; }

        IEnumerable<MvcRoute> ApplyTo { get; }

        Pagination Execute(QueryContext context);
    }

    public static class TabQueries
    {
        public static ITabQuery GetQuery(string name)
        {
            return EngineContext.Current.ResolveAll<ITabQuery>().FirstOrDefault(q => q.Name == name);
        }

        public static IEnumerable<ITabQuery> GetQueries(ControllerContext controllerContext)
        {
            foreach (var query in EngineContext.Current.ResolveAll<ITabQuery>())
            {
                if (query.ApplyTo.Any(x => x.Matches(controllerContext)))
                {
                    yield return query;
                }
            }
        }
    }
}
