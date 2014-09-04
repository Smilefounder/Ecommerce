using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.Api.Brands
{
    public class Brand
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public string Logo { get; set; }

        [OptionalInclude]
        public IDictionary<string, string> CustomFields { get; set; }

        public Brand()
        {
            CustomFields = new Dictionary<string, string>();
        }
    }
}
