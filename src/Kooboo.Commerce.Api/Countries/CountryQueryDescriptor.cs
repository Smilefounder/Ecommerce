using Kooboo.Commerce.Api.Metadata;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.Api.Countries
{
    public class CountryQueryDescriptor : IQueryDescriptor
    {
        public IEnumerable<FilterDescription> Filters
        {
            get
            {
                return new[] { CountryFilters.ById, CountryFilters.ByName, CountryFilters.ByTwoLetterIsoCode, CountryFilters.ByThreeLetterIsoCode, CountryFilters.ByNumericIsoCode };
            }
        }

        public IEnumerable<string> OptionalIncludeFields
        {
            get { return OptionalIncludeAttribute.GetOptionalIncludeFields(typeof(Country)); }
        }

        public IEnumerable<string> DefaultIncludedFields
        {
            get { return null; }
        }

        public IEnumerable<string> SortFields
        {
            get { return new[] { "Id", "Name" }; }
        }
    }
}
