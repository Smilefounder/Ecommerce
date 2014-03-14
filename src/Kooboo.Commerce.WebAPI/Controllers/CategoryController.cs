using Kooboo.Commerce.Categories;
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
            var objs = Commerce().Category.GetAllCategories();
            return objs;
        }

        // GET api/category/5
        public Category Get(int id)
        {
            return Commerce().Category.GetCategory(id, false);
        }

        [HttpGet]
        public IEnumerable<Category> Children(int id)
        {
            return Commerce().Category.GetSubCategories(id);
        }

        [HttpGet]
        public Category Parents(int id)
        {
            return Commerce().Category.GetCategory(id, true);
        }
    }
}
