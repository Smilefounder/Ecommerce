using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.API.Brands
{
    /// <summary>
    /// brand query
    /// all query filter should return self(this) to support fluent api.
    /// </summary>
    public interface IBrandQuery : ICommerceQuery<Brand>
    {
        /// <summary>
        /// add id filter to query
        /// </summary>
        /// <param name="id">brand id</param>
        /// <returns>brand query</returns>
        IBrandQuery ById(int id);
        /// <summary>
        /// add name filter to query
        /// </summary>
        /// <param name="name">brand name</param>
        /// <returns>brand query</returns>
        IBrandQuery ByName(string name);
    }
}
