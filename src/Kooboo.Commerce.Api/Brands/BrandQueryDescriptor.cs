using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.Api.Brands
{
    public class BrandQueryDescriptor : IQueryDescriptor
    {
        public IEnumerable<FilterDescription> Filters
        {
            get
            {
                return new[] { BrandFilters.ById, BrandFilters.ByName, BrandFilters.ByCustomField };
            }
        }

        public IEnumerable<ParameterDescription> Parameters
        {
            get { return null; }
        }

        public IEnumerable<string> OptionalIncludeFields
        {
            get { return null; }
        }

        public IEnumerable<string> DefaultIncludedFields
        {
            get { return null; }
        }

        public IEnumerable<string> SortFields
        {
            get
            {
                return new[] { "Id", "Name" };
            }
        }
    }
}
