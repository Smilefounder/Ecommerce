using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.Categories
{
    public class CategoryTreeNode
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public string Photo { get; set; }

        public CategoryTreeNode Parent { get; set; }

        public IList<CategoryTreeNode> Children { get; set; }

        public IDictionary<string, string> CustomFields { get; set; }

        public CategoryTreeNode()
        {
            Children = new List<CategoryTreeNode>();
            CustomFields = new Dictionary<string, string>();
        }

        public IEnumerable<CategoryTreeNode> Descendants()
        {
            foreach (var child in Children)
            {
                yield return child;

                foreach (var each in child.Descendants())
                {
                    yield return each;
                }
            }
        }

        public IList<CategoryTreeNode> PathFromRoot()
        {
            var path = new List<CategoryTreeNode>();

            CategoryTreeNode entry = this;

            while (entry != null)
            {
                path.Add(entry);
                entry = entry.Parent;
            }

            path.Reverse();

            return path;
        }
    }
}
