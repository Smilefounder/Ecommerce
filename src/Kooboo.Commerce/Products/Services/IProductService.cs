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

        IQueryable<ProductPrice> ProductPrices();

        ProductPrice GetProductPriceById(int id);

        Product Create(Product product);

        Product Update(Product product);

        void Delete(Product product);

        bool Publish(Product product);

        bool Unpublish(Product product);

        void AddPrice(Product product, ProductPrice price);

        bool RemovePrice(Product product, int priceId);

        bool UpdatePrice(Product product, int priceId, ProductPrice newPrice);
    }
}