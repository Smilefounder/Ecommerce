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
        /// add deleted filter to query
        /// </summary>
        /// <param name="deleted">product deleted</param>
        /// <returns>product query</returns>
        IProductQuery IsDeleted(bool deleted);

        /// <summary>
        /// load product with product type
        /// </summary>
        /// <returns>product query</returns>
        IProductQuery LoadWithProductType();
        /// <summary>
        /// load product with brand
        /// </summary>
        /// <returns>product query</returns>
        IProductQuery LoadWithBrand();
        /// <summary>
        /// load product with product categories
        /// </summary>
        /// <returns>product query</returns>
        IProductQuery LoadWithCategories();
        /// <summary>
        /// load product with product images
        /// </summary>
        /// <returns>product query</returns>
        IProductQuery LoadWithImages();
        /// <summary>
        /// load product with product custom fields
        /// </summary>
        /// <returns>product query</returns>
        IProductQuery LoadWithCustomFields();
        /// <summary>
        /// load product with product price list
        /// </summary>
        /// <returns>product query</returns>
        IProductQuery LoadWithPriceList();
    }
}
