using Kooboo.Commerce.Data;
using Kooboo.Commerce.Locations;
using Kooboo.Web.Mvc.Paging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.Web.Framework.Queries
{
    public interface IQuery
    {
        string Name { get; }

        string DisplayName { get; }

        Type ConfigModelType { get; }

        Type ElementType { get; }

        IPagedList Execute(CommerceInstance instance, int pageIndex, int pageSize, object config);
    }
}
