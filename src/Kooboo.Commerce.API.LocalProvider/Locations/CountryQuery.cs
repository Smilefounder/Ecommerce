using Kooboo.Commerce.API.Categories;
using Kooboo.Commerce.API.Locations;
using Kooboo.Commerce.Locations.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.API.LocalProvider.Locations
{
    public class CountryQuery : LocalCommerceQuery<Country, Kooboo.Commerce.Locations.Country>, ICountryQuery
    {
        private ICountryService _countryService;

        public CountryQuery(ICountryService countryService, IMapper<Country, Kooboo.Commerce.Locations.Country> mapper)
        {
            _countryService = countryService;
            _mapper = mapper;
        }

        protected override IQueryable<Kooboo.Commerce.Locations.Country> CreateQuery()
        {
            return _countryService.Query();
        }

        protected override IQueryable<Kooboo.Commerce.Locations.Country> OrderByDefault(IQueryable<Kooboo.Commerce.Locations.Country> query)
        {
            return query.OrderBy(o => o.Id);
        }

        public ICountryQuery ById(int id)
        {
            CreateQuery();
            _query = _query.Where(o => o.Id == id);
            return this;
        }

        public ICountryQuery ByName(string name)
        {
            CreateQuery();
            _query = _query.Where(o => o.Name == name);
            return this;
        }
        public ICountryQuery ByThreeLetterISOCode(string threeLetterISOCode)
        {
            CreateQuery();
            _query = _query.Where(o => o.ThreeLetterISOCode == threeLetterISOCode);
            return this;
        }

        public ICountryQuery ByTwoLetterISOCode(string twoLetterISOCode)
        {
            CreateQuery();
            _query = _query.Where(o => o.TwoLetterISOCode == twoLetterISOCode);
            return this;
        }

        public ICountryQuery ByNumericISOCode(string numericISOCode)
        {
            CreateQuery();
            _query = _query.Where(o => o.NumericISOCode == numericISOCode);
            return this;
        }

        public override void Create(Country obj)
        {
            if (obj != null)
                _countryService.Create(_mapper.MapFrom(obj));
        }

        public override void Update(Country obj)
        {
            if (obj != null)
                _countryService.Update(_mapper.MapFrom(obj));
        }

        public override void Delete(Country obj)
        {
            if (obj != null)
                _countryService.Delete(_mapper.MapFrom(obj));
        }
    }
}
