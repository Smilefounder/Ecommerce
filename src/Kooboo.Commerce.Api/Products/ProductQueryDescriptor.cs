using Kooboo.Commerce.Api.Metadata;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.Api.Products
{
    public class ProductQueryDescriptor : IQueryDescriptor
    {
        public IEnumerable<FilterDescription> Filters
        {
            get { return new[] { ProductFilters.ById, ProductFilters.ByName, ProductFilters.ByBrand, ProductFilters.ByCategory, ProductFilters.ByCustomField }; }
        }

        public IEnumerable<string> OptionalIncludeFields
        {
            get { return OptionalIncludeAttribute.GetOptionalIncludeFields(typeof(Product)); }
        }

        public IEnumerable<string> DefaultIncludedFields
        {
            get { return null; }
        }

        public IEnumerable<string> SortFields
        {
            get { return new[] { "Id", "Name" }; }
        }
    }
}
