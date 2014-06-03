using Kooboo.Commerce.Web.Areas.CommerceWebAPI.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Kooboo.Commerce.API;
using ProductDto = Kooboo.Commerce.API.Products.Product;

namespace Kooboo.Commerce.Recommendations.Api
{
    public class RecommendationController : CommerceAPIControllerBase
    {
        public IListResource<ProductDto> Get(int productId)
        {
            return Commerce().Recommendations().ByProduct(productId).ToArray();
        }
    }
}
