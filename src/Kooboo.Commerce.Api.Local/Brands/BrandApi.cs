using Kooboo.Commerce.Api.Brands;
using Kooboo.Commerce.Api.Local.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.Api.Local.Brands
{
    public class BrandApi : IBrandApi
    {
        private LocalApiContext _context;

        public BrandApi(LocalApiContext context)
        {
            _context = context;
        }

        public Query<Brand> Query()
        {
            return new Query<Brand>(new BrandQueryExecutor(_context));
        }
    }
}
