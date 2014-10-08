using Kooboo.Commerce.Data;
using Kooboo.Commerce.Events;
using Kooboo.Commerce.Events.Categories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.Categories
{
    class CategoryTreeCacheRefresher : IHandle<CategoryCreated>, IHandle<CategoryUpdated>, IHandle<CategoryDeleted>
    {
        public void Handle(CategoryCreated @event, EventContext context)
        {
            CategoryTree.Remove(context.Instance.Name);
        }

        public void Handle(CategoryUpdated @event, EventContext context)
        {
            CategoryTree.Remove(context.Instance.Name);
        }

        public void Handle(CategoryDeleted @event, EventContext context)
        {
            CategoryTree.Remove(context.Instance.Name);
        }
    }
}
