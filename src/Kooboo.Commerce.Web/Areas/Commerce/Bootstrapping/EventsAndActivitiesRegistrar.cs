using Kooboo.CMS.Common.Runtime.Dependency;
using Kooboo.Commerce.Activities;
using Kooboo.Commerce.Events.Brands;
using Kooboo.Commerce.Events.Categories;
using Kooboo.Commerce.Events.Customers;
using Kooboo.Commerce.Events.Orders;
using Kooboo.Commerce.Events.PaymentMethods;
using Kooboo.Commerce.Events.Products;
using Kooboo.Commerce.Events.ProductTypes;
using Kooboo.Commerce.Events.Promotions;
using Kooboo.Commerce.Events.ShippingMethods;
using Kooboo.Commerce.Events.Carts;
using Kooboo.Commerce.Rules.Activities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Kooboo.Commerce.Web.Areas.Commerce.Bootstrapping
{
    public class EventsAndActivitiesRegistrar : IDependencyRegistrar
    {
        public int Order
        {
            get
            {
                return 100;
            }
        }

        public void Register(IContainerManager containerManager, CMS.Common.Runtime.ITypeFinder typeFinder)
        {
            ActivityEventManager.Instance.RegisterEvents(typeFinder.GetAssemblies().Where(asm => !asm.IsDynamic));
            RegisterActivities(containerManager, typeFinder);
        }

        static void RegisterActivities(IContainerManager containerManager, CMS.Common.Runtime.ITypeFinder typeFinder)
        {
            foreach (var activityType in typeFinder.FindClassesOfType<IActivity>())
            {
                if (activityType.IsClass && !activityType.IsAbstract)
                {
                    containerManager.AddComponent(typeof(IActivity), activityType, activityType.FullName, ComponentLifeStyle.Transient);
                }
            }
        }
    }
}