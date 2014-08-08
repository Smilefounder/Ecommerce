using Kooboo.Commerce.Data;
using Kooboo.Commerce.Products;
using Kooboo.Commerce.Search.Documents;
using System;
using System.Globalization;
using System.Linq;

namespace Kooboo.Commerce.Search.Rebuild
{
    public class ProductIndexSource : IDocumentSource
    {
        public int Count(CommerceInstance instance, Type documentType, CultureInfo culture)
        {
            return Query(instance).Count();
        }

        private IQueryable<Product> Query(CommerceInstance instance)
        {
            return instance.Database.GetRepository<Product>().Query().Where(p => p.IsPublished).OrderBy(p => p.Id);
        }

        public System.Collections.IEnumerable Enumerate(CommerceInstance instance, Type documentType, CultureInfo culture)
        {
            foreach (var data in new BatchedQuery<Product>(Query(instance), 1000))
            {
                var product = data as Product;
                var productType = instance.Database.GetRepository<ProductType>().Find(product.ProductTypeId);
                yield return ProductDocument.CreateFrom(product, productType, culture);
            }
        }
    }
}