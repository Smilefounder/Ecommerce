using Kooboo.Commerce.Api.Metadata;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.Api.Orders
{
    public static class OrderFilters
    {
        public static readonly FilterDescription ById = new FilterDescription("ById", new Int32ParameterDescription("Id"));

        public static readonly FilterDescription ByCustomerId = new FilterDescription("ByCustomerId", new Int32ParameterDescription("CustomerId"));

        public static readonly FilterDescription ByCustomerAccountId = new FilterDescription("ByCustomerAccountId", new StringParameterDescription("CustomerAccountId"));

        public static readonly FilterDescription ByUtcCreatedDate = new FilterDescription("ByUtcCreatedDate", new ParameterDescription("FromDate", typeof(DateTime?)), new ParameterDescription("ToDate", typeof(DateTime?)));

        public static readonly FilterDescription ByOrderStatus = new FilterDescription("ByOrderStatus", new ParameterDescription("OrderStatus", typeof(OrderStatus)));

        public static readonly FilterDescription ByProcessingStatus = new FilterDescription("ByProcessingStatus", new StringParameterDescription("ProcessingStatus"));

        public static readonly FilterDescription ByCouponCode = new FilterDescription("ByCouponCode", new StringParameterDescription("CouponCode"));

        public static readonly FilterDescription ByTotal = new FilterDescription("ByTotal", new ParameterDescription("FromTotal", typeof(decimal?)), new ParameterDescription("ToTotal", typeof(decimal?)));

        public static readonly FilterDescription ByCustomField = new FilterDescription("ByCustomField", new StringParameterDescription("FieldName"), new StringParameterDescription("FieldValue"));
    }
}
