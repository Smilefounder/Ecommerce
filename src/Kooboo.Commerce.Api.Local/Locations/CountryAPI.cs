using Kooboo.CMS.Common.Runtime.Dependency;
using Kooboo.Commerce.Api.Local;
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
        private LocalApiContext _context;
        private ICountryService _service;

        public CountryAPI(LocalApiContext context)
        {
            _context = context;
            _service = _context.ServiceFactory.Countries;
        }

        /// <summary>
        /// create entity query
        /// </summary>
        /// <returns>queryable object</returns>
        protected override IQueryable<Kooboo.Commerce.Locations.Country> CreateQuery()
        {
            return _service.Query();
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
        /// add id filter to query
        /// </summary>
        /// <param name="id">country id</param>
        /// <returns>country query</returns>
        public ICountryQuery ById(int id)
        {
            Query = Query.Where(o => o.Id == id);
            return this;
        }

        /// <summary>
        /// add name filter to query
        /// </summary>
        /// <param name="name">country name</param>
        /// <returns>country query</returns>
        public ICountryQuery ByName(string name)
        {
            Query = Query.Where(o => o.Name == name);
            return this;
        }
        /// <summary>
        /// add three letter ISO code filter to query
        /// </summary>
        /// <param name="threeLetterISOCode">country three letter ISO code</param>
        /// <returns>country query</returns>
        public ICountryQuery ByThreeLetterISOCode(string threeLetterISOCode)
        {
            Query = Query.Where(o => o.ThreeLetterIsoCode == threeLetterISOCode);
            return this;
        }

        /// <summary>
        /// add two letter ISO code filter to query
        /// </summary>
        /// <param name="twoLetterISOCode">country two letter ISO code</param>
        /// <returns>country query</returns>
        public ICountryQuery ByTwoLetterISOCode(string twoLetterISOCode)
        {
            Query = Query.Where(o => o.TwoLetterIsoCode == twoLetterISOCode);
            return this;
        }

        /// <summary>
        /// add numeric ISO code filter to query
        /// </summary>
        /// <param name="numericISOCode">country numeric ISO code</param>
        /// <returns>country query</returns>
        public ICountryQuery ByNumericISOCode(string numericISOCode)
        {
            Query = Query.Where(o => o.NumericIsoCode == numericISOCode);
            return this;
        }
    }
}
