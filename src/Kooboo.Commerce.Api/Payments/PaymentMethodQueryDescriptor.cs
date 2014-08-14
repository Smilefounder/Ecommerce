using Kooboo.Commerce.Api.Metadata;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.Api.Payments
{
    public class PaymentMethodQueryDescriptor : IQueryDescriptor
    {
        public IEnumerable<FilterDescription> Filters
        {
            get { return new[] { PaymentMethodFilters.ById, PaymentMethodFilters.ByName, PaymentMethodFilters.ByUserKey }; }
        }

        public IEnumerable<string> OptionalIncludeFields
        {
            get { return OptionalIncludeAttribute.GetOptionalIncludeFields(typeof(PaymentMethod)); }
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
