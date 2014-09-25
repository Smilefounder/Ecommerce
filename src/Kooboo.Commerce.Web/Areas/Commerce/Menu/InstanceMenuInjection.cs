using Kooboo.Commerce.Data;
using Kooboo.Commerce.Web.Framework.UI.Menu;
using Kooboo.Web.Mvc.Menu;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Kooboo.Commerce.Web.Areas.Commerce.Menu
{
    public class InstanceMenuInjection : CommerceMenuInjection
    {
        public override void Inject(Kooboo.Web.Mvc.Menu.MenuItem menu, System.Web.Mvc.ControllerContext controllerContext)
        {
            if (CommerceInstance.Current == null)
            {
                return;
            }

            menu.Items.Add(Start());
            menu.Items.Add(Catalog());
            menu.Items.Add(Orders());
            menu.Items.Add(BusinessRules());
            menu.Items.Add(Shop());
            menu.Items.Add(Customers());
            menu.Items.Add(System());
        }

        private MenuItem Start()
        {
            return new CommerceMenuItem
            {
                Name = "Start",
                Text = "Start",
                Controller = "Instance",
                Action = "Start"
            };
        }

        private MenuItem Catalog()
        {
            var item = new CommerceMenuItem
            {
                Name = "Catalog",
                Text = "Catalog"
            };

            item.Items.Add(new CommerceMenuItem
            {
                Name = "Categories",
                Text = "Categories",
                Controller = "Category",
                Action = "Index"
            });
            item.Items.Add(new CommerceMenuItem
            {
                Name = "Brands",
                Text = "Brands",
                Controller = "Brand",
                Action = "Index"
            });
            item.Items.Add(new CommerceMenuItem
            {
                Name = "Products",
                Text = "Products",
                Controller = "Product",
                Action = "Index"
            });
            item.Items.Add(new CommerceMenuItem
            {
                Name = "MediaLibrary",
                Text = "Media Library",
                Controller = "MediaLibrary",
                Action = "Index"
            });

            return item;
        }

        private MenuItem Orders()
        {
            return new CommerceMenuItem
            {
                Name = "Orders",
                Text = "Orders",
                Controller = "Order",
                Action = "Index"
            };
        }

        private MenuItem BusinessRules()
        {
            var businessRules = new CommerceMenuItem
            {
                Name = "BusinessRules",
                Text = "Business rules"
            };

            foreach (var item in BusinessRuleMenuItems.GetMenuItems())
            {
                businessRules.Items.Add(item);
            }

            return businessRules;
        }

        private MenuItem Shop()
        {
            var shop = new CommerceMenuItem
            {
                Name = "Shop",
                Text = "Shop"
            };

            shop.Items.Add(new CommerceMenuItem
            {
                Name = "Promotions",
                Text = "Promotions",
                Controller = "Promotion",
                Action = "Index"
            });
            shop.Items.Add(new CommerceMenuItem
            {
                Name = "PaymentMethods",
                Text = "Payment methods",
                Action = "Index",
                Controller = "PaymentMethod"
            });
            shop.Items.Add(new CommerceMenuItem
            {
                Name = "ShippingMethods",
                Text = "Shipping methods",
                Controller = "ShippingMethod",
                Action = "Index"
            });

            return shop;
        }

        private MenuItem Customers()
        {
            return new CommerceMenuItem
            {
                Name = "Customers",
                Text = "Customers",
                Controller = "Customer",
                Action = "Index"
            };
        }

        private MenuItem System()
        {
            var system = new CommerceMenuItem
            {
                Name = "System",
                Text = "System"
            };

            system.Items.Add(new CommerceMenuItem
            {
                Name = "Settings",
                Text = "Settings",
                Controller = "Setting",
                Action = "Index"
            });
            system.Items.Add(new CommerceMenuItem
            {
                Name = "Countries",
                Text = "Countries",
                Controller = "Country",
                Action = "Index"
            });
            system.Items.Add(new CommerceMenuItem
            {
                Name = "ProductTypes",
                Text = "Product types",
                Controller = "ProductType",
                Action = "Index"
            });

            return system;
        }
    }
}