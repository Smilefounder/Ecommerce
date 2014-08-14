using Kooboo.Commerce.Api.Metadata;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.Api.Categories
{
    public static class CategoryFilters
    {
        public static readonly FilterDescription ById = new FilterDescription("ById", new ParameterDescription("Id", typeof(Int32)));

        public static readonly FilterDescription ByName = new FilterDescription("ByName", new ParameterDescription("Name", typeof(String)));

        public static readonly FilterDescription ByParent = new FilterDescription("ByParent", new ParameterDescription("ParentId", typeof(Int32)));

        public static readonly FilterDescription ByCustomField = new FilterDescription("ByCustomField", new ParameterDescription("FieldName", typeof(String)), new ParameterDescription("FieldValue", typeof(String)));
    }
}
