﻿using Kooboo.Commerce.Api.Metadata;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.Api.Brands
{
    public static class BrandFilters
    {
        public static readonly FilterDescription ById = new FilterDescription("ById", new Int32ParameterDescription("Id", true));

        public static readonly FilterDescription ByName = new FilterDescription("ByName", new StringParameterDescription("Name", true));

        public static readonly FilterDescription ByCustomField = new FilterDescription("ByCustomField", new StringParameterDescription("FieldName", true), new StringParameterDescription("FieldValue", true));
    }
}
