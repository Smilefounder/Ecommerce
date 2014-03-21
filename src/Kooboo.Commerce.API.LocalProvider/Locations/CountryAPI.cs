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
    [Dependency(typeof(ICountryAPI), ComponentLifeStyle.Transient)]
    public class CountryAPI : LocalCommerceQuery<Country, Kooboo.Commerce.Locations.Country>, ICountryAPI
    {
        private ICountryService _countryService;
        private IMapper<Country, Kooboo.Commerce.Locations.Country> _mapper;

        public CountryAPI(ICountryService countryService, IMapper<Country, Kooboo.Commerce.Locations.Country> mapper)
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

        protected override Country Map(Commerce.Locations.Country obj)
        {
            return _mapper.MapTo(obj);
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

        public ICountryQuery Query()
        {
            return this;
        }
    }
}
