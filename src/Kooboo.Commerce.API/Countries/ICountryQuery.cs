using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.Api.Countries
{
    /// <summary>
    /// country query
    /// all query filter should return self(this) to support fluent api.
    /// </summary>
    public interface ICountryQuery : ICommerceQuery<Country>
    {
        /// <summary>
        /// add id filter to query
        /// </summary>
        /// <param name="id">country id</param>
        /// <returns>country query</returns>
        ICountryQuery ById(int id);
        /// <summary>
        /// add name filter to query
        /// </summary>
        /// <param name="name">country name</param>
        /// <returns>country query</returns>
        ICountryQuery ByName(string name);
        /// <summary>
        /// add three letter ISO code filter to query
        /// </summary>
        /// <param name="threeLetterISOCode">country three letter ISO code</param>
        /// <returns>country query</returns>
        ICountryQuery ByThreeLetterISOCode(string threeLetterISOCode);
        /// <summary>
        /// add two letter ISO code filter to query
        /// </summary>
        /// <param name="twoLetterISOCode">country two letter ISO code</param>
        /// <returns>country query</returns>
        ICountryQuery ByTwoLetterISOCode(string twoLetterISOCode);
        /// <summary>
        /// add numeric ISO code filter to query
        /// </summary>
        /// <param name="numericISOCode">country numeric ISO code</param>
        /// <returns>country query</returns>
        ICountryQuery ByNumericISOCode(string numericISOCode);
    }
}
