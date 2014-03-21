using Kooboo.Commerce.API;
using Kooboo.Commerce.API.Categories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Kooboo.Commerce.WebAPI.Controllers
{
    public class CategoryController : CommerceAPIControllerQueryBase<Category>
    {
        protected override ICommerceQuery<Category> BuildQueryFromQueryStrings()
        {
            var qs = Request.RequestUri.ParseQueryString();
            var query = Commerce().Categories.Query();
            if (!string.IsNullOrEmpty(qs["id"]))
                query = query.ById(Convert.ToInt32(qs["id"]));
            if (!string.IsNullOrEmpty(qs["name"]))
                query = query.ByName(qs["name"]);
            if (!string.IsNullOrEmpty(qs["published"]))
                query = query.Published(Convert.ToBoolean(qs["published"]));
            if (qs["parentId"] != null)
            {
                if (!string.IsNullOrEmpty(qs["parentId"]))
                    query = query.ByParentId(Convert.ToInt32(qs["parentId"]));
                else
                    query = query.ByParentId(null);
            }

            if (qs["LoadWithParent"] == "true")
                query = query.LoadWithParent();
            if (qs["LoadWithAllParents"] == "true")
                query = query.LoadWithAllParents();
            if (qs["LoadWithChildren"] == "true")
                query = query.LoadWithChildren();

            return query;
        }

    }
}
