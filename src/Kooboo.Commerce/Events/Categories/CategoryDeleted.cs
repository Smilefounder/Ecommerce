using Kooboo.Commerce.Categories;
using Kooboo.Commerce.Rules;
using Kooboo.Commerce.Rules.Activities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.Events.Categories
{
    [ActivityEvent(Order = 300)]
    public class CategoryDeleted : Event, ICategoryEvent
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
