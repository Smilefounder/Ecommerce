using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Kooboo.CMS.Common.Runtime;
using Kooboo.CMS.Common.Runtime.Dependency;
using Kooboo.Commerce.Brands;
using Kooboo.Commerce.Brands.Services;

namespace Kooboo.Commerce.API.LocalAPI
{
    [Dependency(typeof(IBrandAPI), ComponentLifeStyle.Transient, Key = "LocalAPI")]
    public class BrandAPI : IBrandAPI
    {
        private IBrandService _brandService;

        public BrandAPI(IBrandService brandService)
        {
            _brandService = brandService;
        }

        public IEnumerable<Brand> GetAllBrands()
        {
            return _brandService.GetAllBrands();
        }
    }
}
