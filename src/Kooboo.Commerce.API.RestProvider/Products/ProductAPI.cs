using Kooboo.CMS.Common.Runtime.Dependency;
using Kooboo.Commerce.API.Brands;
using Kooboo.Commerce.API.Products;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.API.RestProvider.Products
{
    [Dependency(typeof(IProductAPI), ComponentLifeStyle.Transient)]
    public class ProductAPI : RestApiAccessBase<Product>, IProductAPI
    {
        public IProductQuery ById(int id)
        {
            QueryParameters.Add("id", id.ToString());
            return this;
        }

        public IProductQuery ByCategoryId(int categoryId)
        {
            QueryParameters.Add("categoryId", categoryId.ToString());
            return this;
        }


        public IProductQuery ByName(string name)
        {
            QueryParameters.Add("name", name);
            return this;
        }

        public IProductQuery ContainsName(string name)
        {
            QueryParameters.Add("containsName", name);
            return this;
        }

        public IProductQuery ByProductTypeId(int productTypeId)
        {
            QueryParameters.Add("productTypeId", productTypeId.ToString());
            return this;
        }

        public IProductQuery ByBrandId(int brandId)
        {
            QueryParameters.Add("brandId", brandId.ToString());
            return this;
        }

        public IProductQuery IsPublished(bool published)
        {
            QueryParameters.Add("published", published.ToString());
            return this;
        }

        public IProductQuery IsDeleted(bool deleted)
        {
            QueryParameters.Add("deleted", deleted.ToString());
            return this;
        }
        public IProductQuery LoadWithProductType()
        {
            QueryParameters.Add("LoadWithProductType", "true");
            return this;
        }

        public IProductQuery LoadWithBrand()
        {
            QueryParameters.Add("LoadWithBrand", "true");
            return this;
        }

        public IProductQuery LoadWithCategories()
        {
            QueryParameters.Add("LoadWithCategories", "true");
            return this;
        }

        public IProductQuery LoadWithImages()
        {
            QueryParameters.Add("LoadWithImages", "true");
            return this;
        }

        public IProductQuery LoadWithCustomFields()
        {
            QueryParameters.Add("LoadWithCustomFields", "true");
            return this;
        }

        public IProductQuery LoadWithPriceList()
        {
            QueryParameters.Add("LoadWithPriceList", "true");
            return this;
        }

        public IProductQuery Query()
        {
            return this;
        }

        public IProductAccess Access()
        {
            return this;
        }
    }
}
