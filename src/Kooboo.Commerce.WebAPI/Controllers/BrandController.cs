using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Kooboo.Commerce.Brands;

namespace Kooboo.Commerce.WebAPI.Controllers
{
    public class BrandController : CommerceAPIControllerBase
    {
        // GET api/brand
        public IEnumerable<Brand> Get()
        {
            var objs = Commerce().Brand.GetAllBrands();
            return objs;
        }
    }
}
