using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kooboo.CMS.Common.Runtime.Dependency;

namespace Kooboo.Commerce.API
{
    [Dependency(typeof(ICommerceService))]
    public class LocalCommerceService : ICommerceService
    {
        public IEnumerable<string> GetAllCommerceInstances()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Categories.Category> GetAllCategories(string instance, int level = 1)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Categories.Category> GetSubCategories(string instance, int parentCategoryId)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Brands.Brand> GetAllBrands(string instance)
        {
            throw new NotImplementedException();
        }

        public IPagedList<Products.Product> SearchProducts(string instance, string userInputs, int pageIndex = 0, int pageSize = 50)
        {
            throw new NotImplementedException();
        }

        public Products.Product GetProductById(string instace, int id)
        {
            throw new NotImplementedException();
        }
    }
}
