using Kooboo.Commerce.Api.Metadata;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.Api.Orders
{
    public class OrderQueryDescriptor : IQueryDescriptor
    {
        public IEnumerable<FilterDescription> Filters
        {
            get
            {
                return new[] { 
                    OrderFilters.ById, OrderFilters.ByCustomerId, OrderFilters.ByCustomerAccountId, 
                    OrderFilters.ByUtcCreatedDate, OrderFilters.ByOrderStatus, OrderFilters.ByProcessingStatus, 
                    OrderFilters.ByCouponCode, OrderFilters.ByTotal, OrderFilters.ByCustomField 
                };
            }
        }

        public IEnumerable<string> OptionalIncludeFields
        {
            get { return OptionalIncludeAttribute.GetOptionalIncludeFields(typeof(Order)); }
        }

        public IEnumerable<string> DefaultIncludedFields
        {
            get { return null; }
        }

        public IEnumerable<string> SortFields
        {
            get { return new[] { "Id", "Total" }; }
        }
    }
}
