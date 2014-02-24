using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.Products.Services
{
    public interface IProductService
    {
        Product GetById(int id);

        IPagedList<Product> GetAllProducts(string search, int? pageIndex, int? pageSize);

        IPagedList<ProductPrice> GetAllProductPrices(int? pageIndex, int? pageSize);

        ProductPrice GetProductPriceById(int id, bool loadProduct = true, bool loadVariants = true, bool loadCustomFields = true);

        void Create(Product product);

        void Update(Product product);

        void Delete(Product product);

        void Publish(Product product);

        void Unpublish(Product product);
    }
}