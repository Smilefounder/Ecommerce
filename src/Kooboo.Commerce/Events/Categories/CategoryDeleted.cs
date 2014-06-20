using Kooboo.Commerce.Categories;
using Kooboo.Commerce.Rules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.Events.Categories
{
    [Event(Order = 300, ShortName = "Deleted")]
    public class CategoryDeleted : BusinessEvent, ICategoryEvent
    {
        [Param]
        public int CategoryId { get; set; }

        [Param]
        public string CategoryName { get; set; }

        protected CategoryDeleted() { }

        public CategoryDeleted(Category category)
        {
            CategoryId = category.Id;
            CategoryName = category.Name;
        }
    }
}
