using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.Products.Services
{
    public interface IProductService
    {
        Product GetById(int id);

        IQueryable<Product> Query();

        IQueryable<ProductPrice> ProductPriceQuery();

        IQueryable<ProductCategory> ProductCategoryQuery();
        IQueryable<ProductImage> ProductImageQuery();
        IQueryable<ProductCustomFieldValue> ProductCustomFieldQuery();
        IQueryable<ProductPriceVariantValue> ProductPriceVariantQuery();

        ProductPrice GetProductPriceById(int id, bool loadProduct = true, bool loadVariants = true, bool loadCustomFields = true);

        bool Create(Product product);

        bool Update(Product product);

        bool Delete(Product product);

        bool Save(Product product);

        void Publish(Product product);

        void Unpublish(Product product);
    }
}