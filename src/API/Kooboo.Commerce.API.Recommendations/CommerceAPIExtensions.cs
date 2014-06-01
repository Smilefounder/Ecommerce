using Kooboo.CMS.Common.Runtime;
using Kooboo.Commerce.API.Recommendations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.API
{
    public static class CommerceAPIExtensions
    {
        public static IRecommendationQuery Recommendations(this ICommerceAPI api)
        {
            return EngineContext.Current.Resolve<IRecommendationQuery>();
        }
    }
}
