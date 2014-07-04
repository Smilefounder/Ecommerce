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
using Kooboo.Commerce.Events.ShoppingCarts;
using Kooboo.Commerce.Rules.Activities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Kooboo.Commerce.Web.Areas.Commerce.Bootstrapping
{
    public class ActivityRegistrar : IDependencyRegistrar
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
            RegisterEvents();
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

        static void RegisterEvents()
        {
            var manager = ActivityEventManager.Instance;

            RegisterBrandEvents(manager);
            RegisterCategoryEvents(manager);
            RegisterProductTypeEvents(manager);
            RegisterProductEvents(manager);
            RegisterCustomerEvents(manager);
            RegisterPromotionEvents(manager);
            RegisterPaymentMethodEvents(manager);
            RegisterShippingMethodEvents(manager);
            RegisterShoppingCartEvents(manager);
            RegisterOrderEvents(manager);
        }

        static void RegisterBrandEvents(ActivityEventManager manager)
        {
            manager.RegisterEvent("Brands", "Brand Created", "Created", typeof(BrandCreated));
            manager.RegisterEvent("Brands", "Brand Updated", "Updated", typeof(BrandUpdated));
            manager.RegisterEvent("Brands", "Brand Deleted", "Deleted", typeof(BrandDeleted));
        }

        static void RegisterCategoryEvents(ActivityEventManager manager)
        {
            manager.RegisterEvent("Categories", "Category Created", "Created", typeof(CategoryCreated));
            manager.RegisterEvent("Categories", "Category Updated", "Updated", typeof(CategoryUpdated));
            manager.RegisterEvent("Categories", "Category Deleted", "Deleted", typeof(CategoryDeleted));
        }

        static void RegisterProductTypeEvents(ActivityEventManager manager)
        {
            manager.RegisterEvent("ProductTypes", "Product Type Enabled", "Enabled", typeof(ProductTypeEnabled));
            manager.RegisterEvent("ProductTypes", "Product Type Disabled", "Disabled", typeof(ProductTypeDisabled));
        }

        static void RegisterProductEvents(ActivityEventManager manager)
        {
            manager.RegisterEvent("Products", "Get Price", "GetPrice", typeof(GetPrice));
            manager.RegisterEvent("Products", "Product Created", "Created", typeof(ProductCreated));
            manager.RegisterEvent("Products", "Product Variant Added", "Variant Added", typeof(ProductVariantAdded));
            manager.RegisterEvent("Products", "Product Variant Updated", "Variant Updated", typeof(ProductVariantUpdated));
            manager.RegisterEvent("Products", "Product Variant Deleted", "Variant Deleted", typeof(ProductVariantDeleted));
            manager.RegisterEvent("Products", "Product Updated", "Updated", typeof(ProductUpdated));
            manager.RegisterEvent("Products", "Product Published", "Published", typeof(ProductPublished));
            manager.RegisterEvent("Products", "Product Unpublished", "Unpublished", typeof(ProductUnpublished));
            manager.RegisterEvent("Products", "Product Deleted", "Deleted", typeof(ProductDeleted));
        }

        static void RegisterCustomerEvents(ActivityEventManager manager)
        {
            manager.RegisterEvent("Customers", "Customer Created", "Created", typeof(CustomerCreated));
            manager.RegisterEvent("Customers", "Customer Updated", "Updated", typeof(CustomerUpdated));
            manager.RegisterEvent("Customers", "Customer Deleted", "Deleted", typeof(CustomerDeleted));
        }

        static void RegisterPromotionEvents(ActivityEventManager manager)
        {
            manager.RegisterEvent("Promotions", "Promotion Enabled", "Enabled", typeof(PromotionEnabled));
            manager.RegisterEvent("Promotions", "Promotion Disabled", "Disabled", typeof(PromotionDisabled));
        }

        static void RegisterPaymentMethodEvents(ActivityEventManager manager)
        {
            manager.RegisterEvent("PaymentMethods", "PaymentMethod Enabled", "Enabled", typeof(PaymentMethodEnabled));
            manager.RegisterEvent("PaymentMethods", "PaymentMethod Disabled", "Disabled", typeof(PaymentMethodDisabled));
        }

        static void RegisterShippingMethodEvents(ActivityEventManager manager)
        {
            manager.RegisterEvent("ShippingMethods", "ShippingMethod Enabled", "Enabled", typeof(ShippingMethodEnabled));
            manager.RegisterEvent("ShippingMethods", "ShippingMethod Disabled", "Disabled", typeof(ShippingMethodDisabled));
        }

        static void RegisterShoppingCartEvents(ActivityEventManager manager)
        {
            manager.RegisterEvent("ShoppingCarts", "Cart Created", "Created", typeof(CartCreated));
            manager.RegisterEvent("ShoppingCarts", "Item Added", "Item Added", typeof(CartItemAdded));
            manager.RegisterEvent("ShoppingCarts", "Item Quantity Changed", "Item Quantity Changed", typeof(CartItemQuantityChanged));
            manager.RegisterEvent("ShoppingCarts", "Item Removed", "Item Removed", typeof(CartItemRemoved));
            manager.RegisterEvent("ShoppingCarts", "Cart Price Calculated", "Price Calculated", typeof(CartPriceCalculated));
            manager.RegisterEvent("ShoppingCarts", "Cart Expired", "Cart Expired", typeof(CartExpired));
        }

        static void RegisterOrderEvents(ActivityEventManager manager)
        {
            manager.RegisterEvent("Orders", "Order Created", "Created", typeof(OrderCreated));
            manager.RegisterEvent("Orders", "Order Status Changed", "Status Changed", typeof(OrderStatusChanged));
            manager.RegisterEvent("Orders", "Payment Status Changed", "Payment Status Changed", typeof(PaymentStatusChanged));
        }
    }
}