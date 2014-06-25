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
    /// <summary>
    /// country api
    /// </summary>
    [Dependency(typeof(ICountryAPI), ComponentLifeStyle.Transient)]
    [Dependency(typeof(ICountryQuery), ComponentLifeStyle.Transient)]
    public class CountryAPI : LocalCommerceQuery<Country, Kooboo.Commerce.Locations.Country>, ICountryAPI
    {
        private ICountryService _countryService;

        public CountryAPI(ICountryService countryService, IMapper<Country, Kooboo.Commerce.Locations.Country> mapper)
            : base(mapper)
        {
            _countryService = countryService;
        }

        /// <summary>
        /// create entity query
        /// </summary>
        /// <returns>queryable object</returns>
        protected override IQueryable<Kooboo.Commerce.Locations.Country> CreateQuery()
        {
            return _countryService.Query();
        }

        /// <summary>
        /// use the default order when pagination the query
        /// </summary>
        /// <param name="query">pagination query</param>
        /// <returns>ordered query</returns>
        protected override IQueryable<Kooboo.Commerce.Locations.Country> OrderByDefault(IQueryable<Kooboo.Commerce.Locations.Country> query)
        {
            return query.OrderBy(o => o.Id);
        }

        /// <summary>
        /// map the entity to object
        /// </summary>
        /// <param name="obj">entity</param>
        /// <returns>object</returns>
        protected override Country Map(Commerce.Locations.Country obj)
        {
            return _mapper.MapTo(obj);
        }

        /// <summary>
        /// add id filter to query
        /// </summary>
        /// <param name="id">country id</param>
        /// <returns>country query</returns>
        public ICountryQuery ById(int id)
        {
            EnsureQuery();
            _query = _query.Where(o => o.Id == id);
            return this;
        }

        /// <summary>
        /// add name filter to query
        /// </summary>
        /// <param name="name">country name</param>
        /// <returns>country query</returns>
        public ICountryQuery ByName(string name)
        {
            EnsureQuery();
            _query = _query.Where(o => o.Name == name);
            return this;
        }
        /// <summary>
        /// add three letter ISO code filter to query
        /// </summary>
        /// <param name="threeLetterISOCode">country three letter ISO code</param>
        /// <returns>country query</returns>
        public ICountryQuery ByThreeLetterISOCode(string threeLetterISOCode)
        {
            EnsureQuery();
            _query = _query.Where(o => o.ThreeLetterIsoCode == threeLetterISOCode);
            return this;
        }

        /// <summary>
        /// add two letter ISO code filter to query
        /// </summary>
        /// <param name="twoLetterISOCode">country two letter ISO code</param>
        /// <returns>country query</returns>
        public ICountryQuery ByTwoLetterISOCode(string twoLetterISOCode)
        {
            EnsureQuery();
            _query = _query.Where(o => o.TwoLetterIsoCode == twoLetterISOCode);
            return this;
        }

        /// <summary>
        /// add numeric ISO code filter to query
        /// </summary>
        /// <param name="numericISOCode">country numeric ISO code</param>
        /// <returns>country query</returns>
        public ICountryQuery ByNumericISOCode(string numericISOCode)
        {
            EnsureQuery();
            _query = _query.Where(o => o.NumericIsoCode == numericISOCode);
            return this;
        }

        /// <summary>
        /// create country query
        /// </summary>
        /// <returns>country query</returns>
        public ICountryQuery Query()
        {
            return this;
        }
    }
}
