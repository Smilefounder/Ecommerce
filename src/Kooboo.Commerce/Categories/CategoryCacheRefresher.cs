using Kooboo.Commerce.Data;
using Kooboo.Commerce.Events;
using Kooboo.Commerce.Events.Categories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.Categories
{
    class CategoryCacheRefresher : IHandle<CategoryCreated>, IHandle<CategoryUpdated>, IHandle<CategoryDeleted>
    {
        public void Handle(CategoryCreated @event)
        {
            CategoryCache.Remove(CommerceInstance.Current.Name);
        }

        public void Handle(CategoryUpdated @event)
        {
            CategoryCache.Remove(CommerceInstance.Current.Name);
        }

        public void Handle(CategoryDeleted @event)
        {
            CategoryCache.Remove(CommerceInstance.Current.Name);
        }
    }
}
