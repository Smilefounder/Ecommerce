using Kooboo.CMS.Common.Runtime.Dependency;
using Kooboo.Commerce.API.Brands;
using Kooboo.Commerce.Api.Products;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.API.RestProvider.Products
{
    /// <summary>
    /// product api
    /// </summary>
    [Dependency(typeof(IProductApi))]
    [Dependency(typeof(IProductQuery))]
    public class ProductAPI : RestApiQueryBase<Product>, IProductApi
    {
        /// <summary>
        /// add id filter to query
        /// </summary>
        /// <param name="id">customer id</param>
        /// <returns>customer query</returns>
        public IProductQuery ById(int id)
        {
            QueryParameters.Add("id", id.ToString());
            return this;
        }

        /// <summary>
        /// add category id filter to query
        /// </summary>
        /// <param name="categoryId">category id</param>
        /// <returns>product query</returns>
        public IProductQuery ByCategoryId(int categoryId)
        {
            QueryParameters.Add("categoryId", categoryId.ToString());
            return this;
        }


        /// <summary>
        /// add name filter to query
        /// </summary>
        /// <param name="name">product name</param>
        /// <returns>product query</returns>
        public IProductQuery ByName(string name)
        {
            QueryParameters.Add("name", name);
            return this;
        }

        /// <summary>
        /// add product type id filter to query
        /// </summary>
        /// <param name="productTypeId">product type id</param>
        /// <returns>product query</returns>
        public IProductQuery ByProductTypeId(int productTypeId)
        {
            QueryParameters.Add("productTypeId", productTypeId.ToString());
            return this;
        }

        /// <summary>
        /// add brand id filter to query
        /// </summary>
        /// <param name="brandId">product brand id</param>
        /// <returns>product query</returns>
        public IProductQuery ByBrandId(int brandId)
        {
            QueryParameters.Add("brandId", brandId.ToString());
            return this;
        }

        /// <summary>
        /// filter the product by custom field value
        /// </summary>
        /// <param name="customFieldId">custom field id</param>
        /// <param name="fieldValue">custom field valule</param>
        /// <returns>product query</returns>
        public IProductQuery ByCustomField(int customFieldId, string fieldValue)
        {
            QueryParameters.Add("customField.id", customFieldId.ToString());
            QueryParameters.Add("customField.value", fieldValue);
            return this;
        }

        /// <summary>
        /// filter the product by custom field value
        /// </summary>
        /// <param name="customFieldName">custom field name</param>
        /// <param name="fieldValue">custom field valule</param>
        /// <returns>product query</returns>
        public IProductQuery ByCustomField(string customFieldName, string fieldValue)
        {
            QueryParameters.Add("customField.name", customFieldName);
            QueryParameters.Add("customField.value", fieldValue);
            return this;
        }

        /// <summary>
        /// filter the product by product price variant
        /// </summary>
        /// <param name="variantName">price variant name</param>
        /// <param name="variantVallue">price variant value</param>
        /// <returns>product query</returns>
        public IProductQuery ByVariantField(string variantName, string variantValue)
        {
            QueryParameters.Add("priceVariant.name", variantName);
            QueryParameters.Add("priceVariant.vallue", variantValue);
            return this;
        }
    }
}
