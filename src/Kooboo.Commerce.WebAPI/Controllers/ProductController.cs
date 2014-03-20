using Kooboo.Commerce.API;
using Kooboo.Commerce.API.Products;
using Kooboo.Web.Mvc.Paging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Kooboo.Commerce.WebAPI.Controllers
{
    public class ProductController : CommerceAPIControllerBase
    {
        public Product Get(int id)
        {
            return Commerce().Products.ById(id).FirstOrDefault();
        }

        [HttpGet]
        public PagedListWrapper<Product> Search(string userInput, int? categoryId, int pageIndex = 0, int pageSize = 50)
        {
            var query = Commerce().Products.ContainsName(userInput);
            if (categoryId.HasValue)
                query = query.ByCategoryId(categoryId.Value);
            int totalItemCount = query.Count();
            var pagedData = query.Pagination(pageIndex, pageSize);
            return new PagedListWrapper<Product>(new PagedList<Product>(pagedData, pageIndex, pageSize, totalItemCount));
        }
    }
}
