using Kooboo.Commerce.Categories;
using Kooboo.Commerce.Rules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.Events.Categories
{
    public class CategoryCreated : ICategoryEvent
    {
        [Reference(typeof(Category))]
        public int CategoryId { get; set; }

        public CategoryCreated() { }

        public CategoryCreated(Category category)
        {
            CategoryId = category.Id;
        }
    }
}
