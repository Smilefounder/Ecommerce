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

        [OptionalInclude]
        public IDictionary<string, string> CustomFields { get; set; }

        public Category()
        {
            Children = new List<Category>();
            CustomFields = new Dictionary<string, string>();
        }

        public IEnumerable<Category> Descendants()
        {
            foreach (var child in Children)
            {
                yield return child;

                foreach (var descendant in child.Descendants())
                {
                    yield return descendant;
                }
            }
        }
    }
}
