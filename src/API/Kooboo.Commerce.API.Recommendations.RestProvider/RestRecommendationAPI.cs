using Kooboo.CMS.Common.Runtime.Dependency;
using Kooboo.Commerce.API.Products;
using Kooboo.Commerce.API.RestProvider;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.API.Recommendations.RestProvider
{
    [Dependency(typeof(IRecommendationQuery))]
    public class RestRecommendationAPI : RestApiQueryBase<Product>, IRecommendationQuery
    {
        protected override string ApiControllerPath
        {
            get
            {
                return "Recommendation";
            }
        }

        public IRecommendationQuery ByProduct(int productId)
        {
            QueryParameters.Add("productId", productId.ToString());
            return this;
        }
    }
}
