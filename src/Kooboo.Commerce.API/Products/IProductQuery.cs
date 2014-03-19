using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.API.Products
{
    public interface IProductQuery : ICommerceQuery<Product>, ICommerceAccess<Product>
    {
        IProductQuery ById(int id);
        IProductQuery ByCategoryId(int categoryId);
        IProductQuery ByName(string name);
        IProductQuery ContainsName(string name);

        IProductQuery LoadWithProductType();
        IProductQuery LoadWithBrand();
        IProductQuery LoadWithCategories();
        IProductQuery LoadWithImages();
        IProductQuery LoadWithCustomFields();
        IProductQuery LoadWithPriceList();
    }
}
