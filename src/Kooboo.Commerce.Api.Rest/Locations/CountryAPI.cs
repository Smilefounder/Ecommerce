using Kooboo.CMS.Common.Runtime.Dependency;
using Kooboo.Commerce.Api.Categories;
using Kooboo.Commerce.Api.Countries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.Api.RestProvider.Locations
{
    /// <summary>
    /// country api
    /// </summary>
    [Dependency(typeof(ICountryApi))]
    [Dependency(typeof(ICountryQuery))]
    public class CountryAPI : RestApiQueryBase<Country>, ICountryApi
    {
        /// <summary>
        /// add id filter to query
        /// </summary>
        /// <param name="id">country id</param>
        /// <returns>country query</returns>
        public ICountryQuery ById(int id)
        {
            QueryParameters.Add("id", id.ToString());
            return this;
        }

        /// <summary>
        /// add name filter to query
        /// </summary>
        /// <param name="name">country name</param>
        /// <returns>country query</returns>
        public ICountryQuery ByName(string name)
        {
            QueryParameters.Add("name", name);
            return this;
        }
        /// <summary>
        /// add three letter ISO code filter to query
        /// </summary>
        /// <param name="threeLetterISOCode">country three letter ISO code</param>
        /// <returns>country query</returns>
        public ICountryQuery ByThreeLetterISOCode(string threeLetterISOCode)
        {
            QueryParameters.Add("threeLetterISOCode", threeLetterISOCode);
            return this;
        }

        /// <summary>
        /// add two letter ISO code filter to query
        /// </summary>
        /// <param name="twoLetterISOCode">country two letter ISO code</param>
        /// <returns>country query</returns>
        public ICountryQuery ByTwoLetterISOCode(string twoLetterISOCode)
        {
            QueryParameters.Add("twoLetterISOCode", twoLetterISOCode);
            return this;
        }

        /// <summary>
        /// add numeric ISO code filter to query
        /// </summary>
        /// <param name="numericISOCode">country numeric ISO code</param>
        /// <returns>country query</returns>
        public ICountryQuery ByNumericISOCode(string numericISOCode)
        {
            QueryParameters.Add("numericISOCode", numericISOCode);
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
