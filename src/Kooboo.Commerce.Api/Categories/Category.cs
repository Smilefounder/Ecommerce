using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.Api.Categories
{
    public class Category
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public string Photo { get; set; }

        public int? ParentId { get; set; }

        [OptionalInclude]
        public IList<Category> Children { get; set; }

        public IDictionary<string, string> CustomFields { get; set; }
    }
}
