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
        public void Handle(CategoryCreated @event, CommerceInstance instance)
        {
            CategoryTree.Remove(CommerceInstance.Current.Name);
        }

        public void Handle(CategoryUpdated @event, CommerceInstance instance)
        {
            CategoryTree.Remove(CommerceInstance.Current.Name);
        }

        public void Handle(CategoryDeleted @event, CommerceInstance instance)
        {
            CategoryTree.Remove(CommerceInstance.Current.Name);
        }
    }
}
