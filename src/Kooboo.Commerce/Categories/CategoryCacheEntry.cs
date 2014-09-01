using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.Categories
{
    public class CategoryCacheEntry
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public CategoryCacheEntry Parent { get; set; }

        public IList<CategoryCacheEntry> Children { get; set; }

        public CategoryCacheEntry()
        {
            Children = new List<CategoryCacheEntry>();
        }

        public IEnumerable<CategoryCacheEntry> Descendants()
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

        public IList<CategoryCacheEntry> PathFromRoot()
        {
            var path = new List<CategoryCacheEntry>();

            CategoryCacheEntry entry = this;

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
