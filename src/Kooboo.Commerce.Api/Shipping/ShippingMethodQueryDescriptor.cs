using Kooboo.Commerce.Api.Metadata;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.Api.Shipping
{
    public class ShippingMethodQueryDescriptor : IQueryDescriptor
    {
        public IEnumerable<FilterDescription> Filters
        {
            get { return new[] { ShippingMethodFilters.ById, ShippingMethodFilters.ByName }; }
        }

        public IEnumerable<string> OptionalIncludeFields
        {
            get { return OptionalIncludeAttribute.GetOptionalIncludeFields(typeof(ShippingMethod)); }
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
