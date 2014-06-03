using Kooboo.Commerce.Web.Areas.CommerceWebAPI.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Kooboo.Commerce.API;
using ProductDto = Kooboo.Commerce.API.Products.Product;
using Kooboo.Commerce.API.Accessories;

namespace Kooboo.Commerce.Accessories.Api
{
    public class AccessoryController : CommerceAPIControllerBase
    {
        public IListResource<ProductDto> Get(int productId)
        {
            return Commerce().Accessories().ByProduct(productId).ToArray();
        }
    }
}
