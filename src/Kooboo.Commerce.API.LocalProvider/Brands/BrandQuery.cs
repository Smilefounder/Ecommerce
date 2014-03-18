using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Kooboo.Commerce.API.Brands;
using Kooboo.Commerce.API.Brands.Services;
using Kooboo.Commerce.Brands.Services;

namespace Kooboo.Commerce.API.LocalProvider.Brands
{
    public class BrandQuery : LocalCommerceQuery<Brand, Kooboo.Commerce.Brands.Brand>, IBrandQuery
    {
        private IBrandService _brandService;

        public BrandQuery(IBrandService brandService, IMapper<Brand, Kooboo.Commerce.Brands.Brand> mapper)
        {
            _brandService = brandService;
            _mapper = mapper;
        }

        protected override IQueryable<Commerce.Brands.Brand> CreateQuery()
        {
            if (_query == null)
                _query = _brandService.Query();
            return _query;
        }

        protected override IQueryable<Commerce.Brands.Brand> OrderByDefault(IQueryable<Commerce.Brands.Brand> query)
        {
            return _query.OrderByDescending(o => o.Id);
        }

        public IBrandQuery ById(int id)
        {
            CreateQuery();
            _query = _query.Where(o => o.Id == id);
            return this;
        }

        public IBrandQuery ByName(string name)
        {
            CreateQuery();
            _query = _query.Where(o => o.Name == name);
            return this;
        }

        public override void Create(Brand obj)
        {
            if (obj != null)
                _brandService.Create(_mapper.MapFrom(obj));
        }

        public override void Update(Brand obj)
        {
            if (obj != null)
                _brandService.Update(_mapper.MapFrom(obj));
        }

        public override void Delete(Brand obj)
        {
            if (obj != null)
                _brandService.Delete(_mapper.MapFrom(obj));
        }
    }
}
