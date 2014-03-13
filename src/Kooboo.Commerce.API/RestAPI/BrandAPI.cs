using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Kooboo.CMS.Common.Runtime;
using Kooboo.CMS.Common.Runtime.Dependency;
using Kooboo.Commerce.Brands;
using Kooboo.Commerce.Brands.Services;

namespace Kooboo.Commerce.API.RestAPI
{
    [Dependency(typeof(IBrandAPI), ComponentLifeStyle.Transient, Key = "RestAPI")]
    public class BrandAPI : RestApiBase, IBrandAPI
    {
        public IEnumerable<Brand> GetAllBrands()
        {
            return Get<List<Brand>>(null);
        }

        protected override string ApiControllerPath
        {
            get { return "Brand"; }
        }
    }
}
