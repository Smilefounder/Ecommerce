using Kooboo.CMS.Common.Runtime.Dependency;
using Kooboo.Commerce.API.Categories;
using Kooboo.Commerce.API.Locations;
using Kooboo.Commerce.Locations.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.API.LocalProvider.Locations
{
    [Dependency(typeof(ICountryQuery), ComponentLifeStyle.Transient)]
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
            EnsureQuery();
            _query = _query.Where(o => o.Id == id);
            return this;
        }

        public ICountryQuery ByName(string name)
        {
            EnsureQuery();
            _query = _query.Where(o => o.Name == name);
            return this;
        }
        public ICountryQuery ByThreeLetterISOCode(string threeLetterISOCode)
        {
            EnsureQuery();
            _query = _query.Where(o => o.ThreeLetterISOCode == threeLetterISOCode);
            return this;
        }

        public ICountryQuery ByTwoLetterISOCode(string twoLetterISOCode)
        {
            EnsureQuery();
            _query = _query.Where(o => o.TwoLetterISOCode == twoLetterISOCode);
            return this;
        }

        public ICountryQuery ByNumericISOCode(string numericISOCode)
        {
            EnsureQuery();
            _query = _query.Where(o => o.NumericISOCode == numericISOCode);
            return this;
        }

        public override bool Create(Country obj)
        {
            if (obj != null)
                return _countryService.Create(_mapper.MapFrom(obj));
            return false;
        }

        public override bool Update(Country obj)
        {
            if (obj != null)
                return _countryService.Update(_mapper.MapFrom(obj));
            return false;
        }

        public override bool Save(Country obj)
        {
            if (obj != null)
                return _countryService.Save(_mapper.MapFrom(obj));
            return false;
        }

        public override bool Delete(Country obj)
        {
            if (obj != null)
                return _countryService.Delete(_mapper.MapFrom(obj));
            return false;
        }
    }
}
