using Kooboo.Commerce.Categories;
using Kooboo.Commerce.Data;
using Kooboo.Commerce.Products;
using Kooboo.Commerce.Search.Models;
using System;
using System.Globalization;
using System.Linq;

namespace Kooboo.Commerce.Search.Rebuild
{
    public class ProductIndexSource : IIndexSource
    {
        public int Count(CommerceInstance instance, CultureInfo culture)
        {
            return Query(instance).Count();
        }

        private IQueryable<Product> Query(CommerceInstance instance)
        {
            return instance.Database.GetRepository<Product>().Query().Where(p => p.IsPublished).OrderBy(p => p.Id);
        }

        public System.Collections.IEnumerable Enumerate(CommerceInstance instance, CultureInfo culture)
        {
            foreach (var data in new BatchedQuery<Product>(Query(instance), 1000))
            {
                var product = data as Product;
                yield return ProductModel.Create(product, culture, CategoryTree.Get(instance.Name));
            }
        }
    }
}