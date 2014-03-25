using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.API.Locations
{
    /// <summary>
    /// country api
    /// </summary>
    public interface ICountryAPI : ICountryQuery
    {
        /// <summary>
        /// create country query
        /// </summary>
        /// <returns>country query</returns>
        ICountryQuery Query();
    }
}
