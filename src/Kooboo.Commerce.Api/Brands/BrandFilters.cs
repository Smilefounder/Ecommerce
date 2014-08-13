using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.Api.Brands
{
    public static class BrandFilters
    {
        public static readonly FilterDescription ById = new FilterDescription("ById", new ParameterDescription("Id", typeof(Int32), null));

        public static readonly FilterDescription ByName = new FilterDescription("ByName", new ParameterDescription("Name", typeof(String), null));

        public static readonly FilterDescription ByCustomField = new FilterDescription("ByCustomField", new ParameterDescription("FieldName", typeof(String), null), new ParameterDescription("FieldValue", typeof(String), null));
    }
}
