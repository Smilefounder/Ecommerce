using Kooboo.Commerce.Api.Metadata;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.Api.Customers
{
    public static class CustomerFilters
    {
        public static readonly FilterDescription ById = new FilterDescription("ById", new Int32ParameterDescription("Id", true));

        public static readonly FilterDescription ByAccountId = new FilterDescription("ByAccountId", new StringParameterDescription("AccountId", true));

        public static readonly FilterDescription ByEmail = new FilterDescription("ByEmail", new StringParameterDescription("Email", true));

        public static readonly FilterDescription ByCustomField = new FilterDescription("ByCustomField", new StringParameterDescription("FieldName", true), new StringParameterDescription("FieldValue", true));
    }
}
