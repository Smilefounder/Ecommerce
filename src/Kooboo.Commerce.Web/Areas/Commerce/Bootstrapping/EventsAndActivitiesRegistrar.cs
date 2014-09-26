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
using Kooboo.Commerce.Rules;

namespace Kooboo.Commerce.Web.Bootstrapping
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
            RegisterActivities(containerManager, typeFinder);

            var manager = EventSlotManager.Instance;

            manager.Register<BrandCreated>("Brands", "Created");
            manager.Register<BrandUpdated>("Brands", "Updated");
            manager.Register<BrandDeleted>("Brands", "Deleted");

            manager.Register<CategoryCreated>("Categories", "Created");
            manager.Register<CategoryUpdated>("Categories", "Updated");
            manager.Register<CategoryDeleted>("Categories", "Deleted");

            manager.Register<ProductTypeEnabled>("Product Types", "Enabled");
            manager.Register<ProductTypeDisabled>("Product Types", "Disabled");

            manager.Register<ProductCreated>("Products", "Created");
            manager.Register<ProductUpdated>("Products", "Updated");
            manager.Register<ProductDeleted>("Products", "Deleted");
            manager.Register<ProductPublished>("Products", "Published");
            manager.Register<ProductUnpublished>("Products", "Unpublished");
            manager.Register<GetPrice>("Products");

            manager.Register<CustomerCreated>("Customers", "Created");
            manager.Register<CustomerUpdated>("Customers", "Updated");
            manager.Register<CustomerDeleted>("Customers", "Deleted");

            manager.Register<PromotionEnabled>("Promotions", "Enabled");
            manager.Register<PromotionDisabled>("Promotions", "Disabled");

            manager.Register<CartItemAdded>("Carts", "Item Added");
            manager.Register<CartItemQuantityChanged>("Carts", "Item Quantity Changed");
            manager.Register<CartItemRemoved>("Carts", "Item Removed");
            manager.Register<CartPriceCalculated>("Carts", "Price Calculated");

            manager.Register<OrderCreated>("Orders", "Created");
            manager.Register<OrderStatusChanged>("Orders", "Status Changed");
            manager.Register<PaymentStatusChanged>("Orders", "Payment Status Changed");

            manager.Register<PaymentMethodEnabled>("Payment Methods", "Enabled");
            manager.Register<PaymentMethodDisabled>("Payment Methods", "Disabled");

            manager.Register<ShippingMethodEnabled>("Shipping Methods", "Enabled");
            manager.Register<ShippingMethodDisabled>("Shipping Methods", "Disabled");
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