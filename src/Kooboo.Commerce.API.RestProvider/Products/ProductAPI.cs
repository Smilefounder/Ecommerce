using Kooboo.CMS.Common.Runtime.Dependency;
using Kooboo.Commerce.API.Brands;
using Kooboo.Commerce.API.Products;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.API.RestProvider.Products
{
    /// <summary>
    /// product api
    /// </summary>
    [Dependency(typeof(IProductAPI), ComponentLifeStyle.Transient)]
    public class ProductAPI : RestApiAccessBase<Product>, IProductAPI
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
        /// add contains name filter to query
        /// product name contains the input
        /// </summary>
        /// <param name="name">product name</param>
        /// <returns>product query</returns>
        public IProductQuery ContainsName(string name)
        {
            QueryParameters.Add("containsName", name);
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
        /// add published filter to query
        /// </summary>
        /// <param name="published">product published</param>
        /// <returns>product query</returns>
        public IProductQuery IsPublished(bool published)
        {
            QueryParameters.Add("published", published.ToString());
            return this;
        }

        /// <summary>
        /// add deleted filter to query
        /// </summary>
        /// <param name="deleted">product deleted</param>
        /// <returns>product query</returns>
        public IProductQuery IsDeleted(bool deleted)
        {
            QueryParameters.Add("deleted", deleted.ToString());
            return this;
        }
        /// <summary>
        /// load product with product type
        /// </summary>
        /// <returns>product query</returns>
        public IProductQuery LoadWithProductType()
        {
            QueryParameters.Add("LoadWithProductType", "true");
            return this;
        }

        /// <summary>
        /// load product with brand
        /// </summary>
        /// <returns>product query</returns>
        public IProductQuery LoadWithBrand()
        {
            QueryParameters.Add("LoadWithBrand", "true");
            return this;
        }

        /// <summary>
        /// load product with product categories
        /// </summary>
        /// <returns>product query</returns>
        public IProductQuery LoadWithCategories()
        {
            QueryParameters.Add("LoadWithCategories", "true");
            return this;
        }

        /// <summary>
        /// load product with product images
        /// </summary>
        /// <returns>product query</returns>
        public IProductQuery LoadWithImages()
        {
            QueryParameters.Add("LoadWithImages", "true");
            return this;
        }

        /// <summary>
        /// load product with product custom fields
        /// </summary>
        /// <returns>product query</returns>
        public IProductQuery LoadWithCustomFields()
        {
            QueryParameters.Add("LoadWithCustomFields", "true");
            return this;
        }

        /// <summary>
        /// load product with product price list
        /// </summary>
        /// <returns>product query</returns>
        public IProductQuery LoadWithPriceList()
        {
            QueryParameters.Add("LoadWithPriceList", "true");
            return this;
        }

        /// <summary>
        /// create product query
        /// </summary>
        /// <returns>product query</returns>
        public IProductQuery Query()
        {
            return this;
        }

        /// <summary>
        /// create product data access
        /// </summary>
        /// <returns>product data access</returns>
        public IProductAccess Access()
        {
            return this;
        }
    }
}
