using Kooboo.CMS.Sites.Models;
using Kooboo.Commerce.API.CmsSite;
using Kooboo.Commerce.API.Products;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.API
{
    public static class ProductExtensions
    {
        public static IEnumerable<Product> GetRecommendations(this Product product)
        {
            return Site.Current.Commerce().Recommendations().ForProduct(product.Id);
        }
    }
}
