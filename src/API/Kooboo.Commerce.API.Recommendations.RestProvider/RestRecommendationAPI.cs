using Kooboo.CMS.Common.Runtime.Dependency;
using Kooboo.Commerce.API.Products;
using Kooboo.Commerce.API.RestProvider;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.API.Recommendations.RestProvider
{
    [Dependency(typeof(IRecommendationAPI))]
    public class RestRecommendationAPI : RestApiBase, IRecommendationAPI
    {
        public Product[] ForProduct(int productId)
        {
            QueryParameters.Add("productId", productId.ToString());
            return Get<Product[]>(null);
        }

        protected override string ApiControllerPath
        {
            get
            {
                return "Recommendation";
            }
        }
    }
}
