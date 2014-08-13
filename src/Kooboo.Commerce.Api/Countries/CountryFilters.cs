using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.Api.Countries
{
    public static class CountryFilters
    {
        public static readonly FilterDescription ById = new FilterDescription("ById", new Int32ParameterDescription("Id"));

        public static readonly FilterDescription ByName = new FilterDescription("ByName", new StringParameterDescription("Name"));

        public static readonly FilterDescription ByThreeLetterIsoCode = new FilterDescription("ByThreeLetterIsoCode", new StringParameterDescription("ThreeLetterIsoCode"));

        public static readonly FilterDescription ByTwoLetterIsoCode = new FilterDescription("ByTwoLetterIsoCode", new StringParameterDescription("TwoLetterIsoCode"));

        public static readonly FilterDescription ByNumericIsoCode = new FilterDescription("ByNumericIsoCode", new StringParameterDescription("NumericIsoCode"));
    }
}
