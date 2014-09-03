using Kooboo.Commerce.Api.Metadata;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.Api.Carts
{
    public class ShoppingCartQueryDescriptor : IQueryDescriptor
    {
        public IEnumerable<FilterDescription> Filters
        {
            get
            {
                return new[] { ShoppingCartFilters.ById, ShoppingCartFilters.BySessionId };
            }
        }

        public IEnumerable<string> OptionalIncludeFields
        {
            get { return OptionalIncludeAttribute.GetOptionalIncludeFields(typeof(ShoppingCart)); }
        }

        public IEnumerable<string> DefaultIncludedFields
        {
            get { return new[] { "Items" }; }
        }

        public IEnumerable<string> SortFields
        {
            get { return null; }
        }
    }
}
