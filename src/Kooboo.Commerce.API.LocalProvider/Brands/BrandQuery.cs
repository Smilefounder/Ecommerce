using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Kooboo.Commerce.API.Brands;
using Kooboo.Commerce.API.Brands.Services;
using Kooboo.Commerce.Brands.Services;
using Kooboo.CMS.Common.Runtime.Dependency;

namespace Kooboo.Commerce.API.LocalProvider.Brands
{
    [Dependency(typeof(IBrandQuery), ComponentLifeStyle.Transient)]
    public class BrandQuery : LocalCommerceQuery<Brand, Kooboo.Commerce.Brands.Brand>, IBrandQuery
    {
        private IBrandService _brandService;
        private IMapper<Brand, Kooboo.Commerce.Brands.Brand> _mapper;

        public BrandQuery(IBrandService brandService, IMapper<Brand, Kooboo.Commerce.Brands.Brand> mapper)
        {
            _brandService = brandService;
            _mapper = mapper;
        }

        protected override IQueryable<Commerce.Brands.Brand> CreateQuery()
        {
            return _brandService.Query();
        }

        protected override IQueryable<Commerce.Brands.Brand> OrderByDefault(IQueryable<Commerce.Brands.Brand> query)
        {
            return _query.OrderByDescending(o => o.Id);
        }

        protected override Brand Map(Commerce.Brands.Brand obj)
        {
            return _mapper.MapTo(obj);
        }

        public IBrandQuery ById(int id)
        {
            EnsureQuery();
            _query = _query.Where(o => o.Id == id);
            return this;
        }

        public IBrandQuery ByName(string name)
        {
            EnsureQuery();
            _query = _query.Where(o => o.Name == name);
            return this;
        }
    }
}
