using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.API.Categories
{
    /// <summary>
    /// category api, query only
    /// </summary>
    public interface ICategoryAPI : ICategoryQuery
    {
        /// <summary>
        /// create category query 
        /// </summary>
        /// <returns>category query</returns>
        ICategoryQuery Query();
    }
}
