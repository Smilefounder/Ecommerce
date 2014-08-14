using Kooboo.Commerce.Api.Metadata;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.Api.Countries
{
    public static class CountryFilters
    {
        public static readonly FilterDescription ById = new FilterDescription("ById", new Int32ParameterDescription("Id", true));

        public static readonly FilterDescription ByName = new FilterDescription("ByName", new StringParameterDescription("Name", true));

        public static readonly FilterDescription ByThreeLetterIsoCode = new FilterDescription("ByThreeLetterIsoCode", new StringParameterDescription("ThreeLetterIsoCode", true));

        public static readonly FilterDescription ByTwoLetterIsoCode = new FilterDescription("ByTwoLetterIsoCode", new StringParameterDescription("TwoLetterIsoCode", true));

        public static readonly FilterDescription ByNumericIsoCode = new FilterDescription("ByNumericIsoCode", new StringParameterDescription("NumericIsoCode", true));
    }
}
