using Kooboo.Commerce.Categories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Kooboo.Commerce.Web.Areas.Commerce.Models.Categories
{
    public class CategoryEntry
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public int? ParentId { get; set; }

        public List<CategoryEntry> Children { get; set; }

        public CategoryEntry()
        {
            Children = new List<CategoryEntry>();
        }

        public static List<CategoryEntry> BuildCategoryTree(int? parentId, IEnumerable<CategoryEntry> all)
        {
            IEnumerable<CategoryEntry> categories = null;

            if (parentId == null)
            {
                categories = all.Where(c => c.ParentId == null);
            }
            else
            {
                categories = all.Where(c => c.ParentId == parentId);
            }

            var entries = new List<CategoryEntry>();
            foreach (var root in categories)
            {
                var entry = new CategoryEntry
                {
                    Id = root.Id,
                    Name = root.Name,
                    ParentId = parentId
                };

                entry.Children = BuildCategoryTree(entry.Id, all);

                entries.Add(entry);
            }

            return entries;
        }
    }
}