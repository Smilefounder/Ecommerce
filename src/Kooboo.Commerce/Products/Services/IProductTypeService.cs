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

        bool Create(ProductType type);

        bool Update(ProductType type);

        bool Delete(int productTypeId);

        void Enable(ProductType type);

        void Disable(ProductType type);
    }
}