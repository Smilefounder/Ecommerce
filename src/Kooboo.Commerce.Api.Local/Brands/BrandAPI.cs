using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Kooboo.Commerce.Api.Brands;
using Kooboo.Commerce.Brands.Services;
using Kooboo.CMS.Common.Runtime.Dependency;
using System.Globalization;

namespace Kooboo.Commerce.Api.Local.Brands
{
    /// <summary>
    /// brand api
    /// </summary>
    [Dependency(typeof(IBrandApi), ComponentLifeStyle.Transient)]
    [Dependency(typeof(IBrandQuery), ComponentLifeStyle.Transient)]
    public class BrandApi : LocalCommerceQuery<Brand, Kooboo.Commerce.Brands.Brand>, IBrandApi
    {
        private IBrandService _brandService;

        public BrandApi(IBrandService brandService)
        {
            _brandService = brandService;
        }

        /// <summary>
        /// create entity query
        /// </summary>
        /// <returns>queryable object</returns>
        protected override IQueryable<Commerce.Brands.Brand> CreateQuery()
        {
            return _brandService.Query();
        }

        /// <summary>
        /// use the default order when pagination the query
        /// </summary>
        /// <param name="query">pagination query</param>
        /// <returns>ordered query</returns>
        protected override IQueryable<Commerce.Brands.Brand> OrderByDefault(IQueryable<Commerce.Brands.Brand> query)
        {
            return query.OrderByDescending(o => o.Id);
        }

        /// <summary>
        /// add id filter to query
        /// </summary>
        /// <param name="id">brand id</param>
        /// <returns>brand query</returns>
        public IBrandQuery ById(int id)
        {
            Query = Query.Where(o => o.Id == id);
            return this;
        }

        /// <summary>
        /// add name filter to query
        /// </summary>
        /// <param name="name">brand name</param>
        /// <returns>brand query</returns>
        public IBrandQuery ByName(string name)
        {
            Query = Query.Where(o => o.Name == name);
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
            var customFieldQuery = _brandService.CustomFields().Where(o => o.Name == customFieldName && o.Value == fieldValue);
            Query = Query.Where(o => customFieldQuery.Any(c => c.BrandId == o.Id));
            return this;
        }
    }
}
