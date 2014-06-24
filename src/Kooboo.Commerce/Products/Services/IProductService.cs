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

        IQueryable<ProductPrice> QueryProductPrices();

        ProductPrice GetProductPriceById(int id, bool loadProduct = true, bool loadVariants = true, bool loadCustomFields = true);

        bool Create(Product product);

        bool Delete(int productId);

        bool Publish(Product product);

        bool Unpublish(Product product);

        void AddPrice(Product product, ProductPrice price);

        bool RemovePrice(Product product, int priceId);

        bool UpdatePrice(Product product, int priceId, ProductPrice newPrice);
    }
}