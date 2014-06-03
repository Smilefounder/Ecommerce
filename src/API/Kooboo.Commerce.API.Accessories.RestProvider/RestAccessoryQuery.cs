using Kooboo.CMS.Common.Runtime.Dependency;
using Kooboo.Commerce.API.Products;
using Kooboo.Commerce.API.RestProvider;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.API.Accessories.RestProvider
{
    [Dependency(typeof(IAccessoryQuery))]
    public class RestAccessoryQuery : RestApiQueryBase<Product>, IAccessoryQuery
    {
        protected override string ApiControllerPath
        {
            get
            {
                return "Accessory";
            }
        }

        public IAccessoryQuery ByProduct(int productId)
        {
            QueryParameters.Add("productId", productId.ToString());
            return this;
        }
    }
}
