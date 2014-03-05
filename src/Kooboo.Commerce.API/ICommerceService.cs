using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kooboo;
using Kooboo.Commerce.Accounts;
using Kooboo.Commerce.Brands;
using Kooboo.Commerce.Categories;
using Kooboo.Commerce.Customers;
using Kooboo.Commerce.EAV;
using Kooboo.Commerce.Events;
using Kooboo.Commerce.Locations;
using Kooboo.Commerce.Orders;
using Kooboo.Commerce.Payments;
using Kooboo.Commerce.Pricing;
using Kooboo.Commerce.Products;
using Kooboo.Commerce.Promotions;
using Kooboo.Commerce.ShoppingCarts;

namespace Kooboo.Commerce.API
{
    public interface ICommerceService
    {
        IEnumerable<string> GetAllCommerceInstances();

        IEnumerable<Category> GetAllCategories(string instance, int level = 1);

        IEnumerable<Category> GetSubCategories(string instance, int parentCategoryId);

        IEnumerable<Brand> GetAllBrands(string instance);

        IPagedList<Product> SearchProducts(string instance, string userInputs, int pageIndex = 0, int pageSize = 50);

        Product GetProductById(string instace, int id);
    }
}
