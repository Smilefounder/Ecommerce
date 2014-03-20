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
            var objs = Commerce().Categories.ToArray();
            return objs;
        }

        // GET api/category/5
        public Category Get(int id)
        {
            return Commerce().Categories.ById(id).FirstOrDefault();
        }

        [HttpGet]
        public IEnumerable<Category> Children(int id)
        {
            return Commerce().Categories.ByParentId(id).ToArray();
        }

        [HttpGet]
        public Category Parents(int id)
        {
            return Commerce().Categories.ById(id).LoadWithAllParents().FirstOrDefault();
        }
    }
}
