using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Kooboo.Commerce.Brands;

namespace Kooboo.Commerce.API
{
    public interface IBrandAPI
    {
        IEnumerable<Brand> GetAllBrands();
    }
}
