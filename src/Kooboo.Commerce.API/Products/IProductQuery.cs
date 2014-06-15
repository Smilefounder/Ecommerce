using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.API.Products
{
    /// <summary>
    /// product query
    /// all query filter should return self(this) to support fluent api.
    /// </summary>
    public interface IProductQuery : ICommerceQuery<Product>
    {
        /// <summary>
        /// add id filter to query
        /// </summary>
        /// <param name="id">product id</param>
        /// <returns>product query</returns>
        IProductQuery ById(int id);
        /// <summary>
        /// add category id filter to query
        /// </summary>
        /// <param name="categoryId">category id</param>
        /// <returns>product query</returns>
        IProductQuery ByCategoryId(int categoryId);
        /// <summary>
        /// add name filter to query
        /// </summary>
        /// <param name="name">product name</param>
        /// <returns>product query</returns>
        IProductQuery ByName(string name);
        /// <summary>
        /// add contains name filter to query
        /// product name contains the input
        /// </summary>
        /// <param name="name">product name</param>
        /// <returns>product query</returns>
        IProductQuery ContainsName(string name);
        /// <summary>
        /// add product type id filter to query
        /// </summary>
        /// <param name="productTypeId">product type id</param>
        /// <returns>product query</returns>
        IProductQuery ByProductTypeId(int productTypeId);
        /// <summary>
        /// add brand id filter to query
        /// </summary>
        /// <param name="brandId">product brand id</param>
        /// <returns>product query</returns>
        IProductQuery ByBrandId(int brandId);
        /// <summary>
        /// add published filter to query
        /// </summary>
        /// <param name="published">product published</param>
        /// <returns>product query</returns>
        IProductQuery IsPublished(bool published);

        /// <summary>
        /// filter the product by custom field value
        /// </summary>
        /// <param name="customFieldId">custom field id</param>
        /// <param name="fieldValue">custom field valule</param>
        /// <returns>product query</returns>
        IProductQuery ByCustomField(int customFieldId, string fieldValue);

        /// <summary>
        /// filter the product by custom field value
        /// </summary>
        /// <param name="customFieldName">custom field name</param>
        /// <param name="fieldValue">custom field valule</param>
        /// <returns>product query</returns>
        IProductQuery ByCustomField(string customFieldName, string fieldValue);

        /// <summary>
        /// filter the product by product price variant
        /// </summary>
        /// <param name="variantId">price variant id</param>
        /// <param name="variantVallue">price variant value</param>
        /// <returns>product query</returns>
        IProductQuery ByPriceVariant(int variantId, string variantVallue);
        /// <summary>
        /// filter the product by product price variant
        /// </summary>
        /// <param name="variantName">price variant name</param>
        /// <param name="variantVallue">price variant value</param>
        /// <returns>product query</returns>
        IProductQuery ByPriceVariant(string variantName, string variantValue);
    }
}
