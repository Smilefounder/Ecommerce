using Kooboo.Commerce.Categories;
using Kooboo.Commerce.Rules;
using Kooboo.Commerce.Rules.Activities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.Events.Categories
{
    [ActivityEvent(Order = 100)]
    public class CategoryCreated : Event, ICategoryEvent
    {
        [Reference(typeof(Category))]
        public int CategoryId { get; set; }

        protected CategoryCreated() { }

        public CategoryCreated(Category category)
        {
            CategoryId = category.Id;
        }
    }
}
