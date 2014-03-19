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
    public class BrandQuery : LocalCommerceQueryAccess<Brand, Kooboo.Commerce.Brands.Brand>, IBrandQuery
    {
        private IBrandService _brandService;

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

        public override bool Create(Brand obj)
        {
            if (obj != null)
                return _brandService.Create(_mapper.MapFrom(obj));
            return false;
        }

        public override bool Update(Brand obj)
        {
            if (obj != null)
                return _brandService.Update(_mapper.MapFrom(obj));
            return false;
        }

        public override bool Save(Brand obj)
        {
            if (obj != null)
                return _brandService.Save(_mapper.MapFrom(obj));
            return false;
        }

        public override bool Delete(Brand obj)
        {
            if (obj != null)
                _brandService.Delete(_mapper.MapFrom(obj));
            return false;
        }
    }
}
