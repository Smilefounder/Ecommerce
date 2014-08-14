using Kooboo.Commerce.Api.Metadata;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.Api.Products
{
    public static class ProductFilters
    {
        public static readonly FilterDescription ById = new FilterDescription("ById", new Int32ParameterDescription("Id"));

        public static readonly FilterDescription ByName = new FilterDescription("ByName", new StringParameterDescription("Name"));

        public static readonly FilterDescription ByCategory = new FilterDescription("ByCategory", new Int32ParameterDescription("CategoryId"));

        public static readonly FilterDescription ByBrand = new FilterDescription("ByBrand", new Int32ParameterDescription("BrandId"));

        public static readonly FilterDescription ByCustomField = new FilterDescription("ByCustomField", new StringParameterDescription("FieldName"), new StringParameterDescription("FieldValue"));
    }
}
