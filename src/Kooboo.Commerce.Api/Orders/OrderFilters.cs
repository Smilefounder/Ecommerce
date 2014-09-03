using Kooboo.Commerce.Api.Metadata;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.Api.Orders
{
    public static class OrderFilters
    {
        public static readonly FilterDescription ById = new FilterDescription("ById", new Int32ParameterDescription("Id", true));

        public static readonly FilterDescription ByCustomerId = new FilterDescription("ByCustomerId", new Int32ParameterDescription("CustomerId", true));

        public static readonly FilterDescription ByUtcCreatedDate = new FilterDescription("ByUtcCreatedDate", new ParameterDescription("FromDate", typeof(DateTime?), false), new ParameterDescription("ToDate", typeof(DateTime?), false));

        public static readonly FilterDescription ByOrderStatus = new FilterDescription("ByOrderStatus", new ParameterDescription("OrderStatus", typeof(OrderStatus), true));

        public static readonly FilterDescription ByProcessingStatus = new FilterDescription("ByProcessingStatus", new StringParameterDescription("ProcessingStatus", true));

        public static readonly FilterDescription ByCouponCode = new FilterDescription("ByCouponCode", new StringParameterDescription("CouponCode", true));

        public static readonly FilterDescription ByTotal = new FilterDescription("ByTotal", new ParameterDescription("FromTotal", typeof(decimal?), false), new ParameterDescription("ToTotal", typeof(decimal?), false));

        public static readonly FilterDescription ByCustomField = new FilterDescription("ByCustomField", new StringParameterDescription("FieldName", true), new StringParameterDescription("FieldValue", true));
    }
}
