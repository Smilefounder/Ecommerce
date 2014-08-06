using Kooboo.Commerce.Data;
using Kooboo.Commerce.Products;
using Kooboo.Commerce.Search.Builders;
using System;
using System.Globalization;
using System.Linq;

namespace Kooboo.Commerce.Search.Rebuild
{
    public class ProductIndexSource : IIndexSource
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
            return new BatchedQuery<Product>(Query(instance), 1000);
        }

        public Lucene.Net.Documents.Document CreateDocument(object data, CommerceInstance instance, Type documentType, CultureInfo culture)
        {
            var product = data as Product;
            var productType = instance.Database.GetRepository<ProductType>().Find(product.ProductTypeId);
            return ProductDocumentBuilder.Build(product, productType, culture);
        }
    }
}