using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kooboo.CMS.Common.Runtime.Dependency;
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
using Kooboo.Commerce.Accounts.Services;
using Kooboo.Commerce.Brands.Services;
using Kooboo.Commerce.Categories.Services;
using Kooboo.Commerce.Customers.Services;
using Kooboo.Commerce.EAV.Services;
using Kooboo.Commerce.Locations.Services;
using Kooboo.Commerce.Orders.Services;
using Kooboo.Commerce.Payments.Services;
using Kooboo.Commerce.Products.Services;
using Kooboo.Commerce.Promotions.Services;
using Kooboo.Commerce.ShoppingCarts.Services;

namespace Kooboo.Commerce.API
{
    [Dependency(typeof(ICommerceService))]
    public class LocalCommerceService : ICommerceService
    {
        private ICategoryService _categorySvr;

        public LocalCommerceService(ICategoryService categorySvr)
        {
            _categorySvr = categorySvr;
        }

        private void InitCommerceInstance(string instance, string language)
        {

        }

        public IEnumerable<Category> GetAllCategories(string instance, string language, int level = 1)
        {
            InitCommerceInstance(instance, language);
            throw new NotImplementedException();
        }

        public IEnumerable<Category> GetSubCategories(string instance, string language, int parentCategoryId)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Brand> GetAllBrands(string instance, string language)
        {
            throw new NotImplementedException();
        }

        public IPagedList<Product> SearchProducts(string instance, string language, string userInputs, int pageIndex = 0, int pageSize = 50)
        {
            throw new NotImplementedException();
        }

        public Product GetProductById(string instace, string language, int id)
        {
            throw new NotImplementedException();
        }
    }
}
