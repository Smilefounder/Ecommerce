using Kooboo.Commerce.Api.Metadata;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.Api.Products
{
    public static class ProductFilters
    {
        public static readonly FilterDescription ById = new FilterDescription("ById", new Int32ParameterDescription("Id", true));

        public static readonly FilterDescription ByIds = new FilterDescription("ByIds", new ParameterDescription("Ids", typeof(int[]), true));

        public static readonly FilterDescription ByName = new FilterDescription("ByName", new StringParameterDescription("Name", true));

        public static readonly FilterDescription ByCategory = new FilterDescription("ByCategory", new Int32ParameterDescription("CategoryId", true));

        public static readonly FilterDescription ByBrand = new FilterDescription("ByBrand", new Int32ParameterDescription("BrandId", true));

        public static readonly FilterDescription ByCustomField = new FilterDescription("ByCustomField", new StringParameterDescription("FieldName", true), new StringParameterDescription("FieldValue", true));
    }
}
