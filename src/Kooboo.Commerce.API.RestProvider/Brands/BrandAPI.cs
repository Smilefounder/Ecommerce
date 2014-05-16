using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Kooboo.Commerce.API.Brands;
using Kooboo.CMS.Common.Runtime.Dependency;

namespace Kooboo.Commerce.API.RestProvider.Brands
{
    /// <summary>
    /// brand api
    /// </summary>
    [Dependency(typeof(IBrandAPI))]
    [Dependency(typeof(IBrandQuery))]
    public class BrandAPI : RestApiAccessBase<Brand>, IBrandAPI
    {
        /// <summary>
        /// add id filter to query
        /// </summary>
        /// <param name="id">brand id</param>
        /// <returns>brand query</returns>
        public IBrandQuery ById(int id)
        {
            QueryParameters.Add("id", id.ToString());
            return this;
        }

        /// <summary>
        /// add name filter to query
        /// </summary>
        /// <param name="name">brand name</param>
        /// <returns>brand query</returns>
        public IBrandQuery ByName(string name)
        {
            QueryParameters.Add("name", name);
            return this;
        }

        /// <summary>
        /// filter by custom field value
        /// </summary>
        /// <param name="customFieldName">custom field name</param>
        /// <param name="fieldValue">custom field valule</param>
        /// <returns>brand query</returns>
        public IBrandQuery ByCustomField(string customFieldName, string fieldValue)
        {
            QueryParameters.Add("customField.name", customFieldName);
            QueryParameters.Add("customField.value", fieldValue);
            return this;
        }

        /// <summary>
        /// create a query
        /// </summary>
        /// <returns>brand query</returns>
        public IBrandQuery Query()
        {
            return this;
        }
    }
}
