using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.Products.Services
{
    public interface IProductTypeService
    {
        ProductType GetById(int id);

        IQueryable<ProductType> Query();

        ProductType Create(CreateProductTypeRequest request);

        ProductType Update(UpdateProductTypeRequest request);

        void Delete(ProductType type);

        bool Enable(ProductType type);

        bool Disable(ProductType type);
    }
}