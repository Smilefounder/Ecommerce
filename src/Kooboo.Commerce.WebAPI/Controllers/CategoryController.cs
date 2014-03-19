using Kooboo.Commerce.API.Categories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Kooboo.Commerce.WebAPI.Controllers
{
    public class CategoryController : CommerceAPIControllerBase
    {
        // GET api/category
        public IEnumerable<Category> Get()
        {
            var objs = Commerce().Category.ToArray();
            return objs;
        }

        // GET api/category/5
        public Category Get(int id)
        {
            return Commerce().Category.ById(id).FirstOrDefault();
        }

        [HttpGet]
        public IEnumerable<Category> Children(int id)
        {
            return Commerce().Category.ByParentId(id).ToArray();
        }

        [HttpGet]
        public Category Parents(int id)
        {
            return Commerce().Category.ById(id).LoadWithAllParents().FirstOrDefault();
        }
    }
}
