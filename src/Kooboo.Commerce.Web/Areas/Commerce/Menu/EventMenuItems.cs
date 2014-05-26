using Kooboo.CMS.Common.Runtime;
using Kooboo.Commerce.Activities;
using Kooboo.Commerce.Events;
using Kooboo.Commerce.Events.Registry;
using Kooboo.Web.Mvc.Menu;
using Kooboo.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Routing;

namespace Kooboo.Commerce.Web.Areas.Commerce.Menu
{
    public class EventMenuItem : MenuItem
    {
        public EventMenuItem(Type eventType)
        {
            Name = eventType.Name;
            Text = eventType.GetDescription() ?? Name.Humanize();

            Controller = "Activity";
            Action = "List";
            Area = "Commerce";

            RouteValues = new System.Web.Routing.RouteValueDictionary();
            RouteValues.Add("eventType", eventType.AssemblyQualifiedNameWithoutVersion());

            Initializer = new EventMenuItemInitializer();
        }
    }

    public class EventMenuItems : IMenuItemContainer
    {
        private IEventRegistry _eventRegistry;

        public EventMenuItems()
            : this(EngineContext.Current.Resolve<IEventRegistry>())
        {
        }

        public EventMenuItems(IEventRegistry eventRegistry)
        {
            Require.NotNull(eventRegistry, "eventRegistry");
            _eventRegistry = eventRegistry;
        }

        public IEnumerable<MenuItem> GetItems(string areaName, System.Web.Mvc.ControllerContext controllerContext)
        {
            var menuItems = new List<MenuItem>();

            foreach (var category in _eventRegistry.AllCategories())
            {
                var categoryMenuItem = new MenuItem
                {
                    Text = category,
                    Name = category,
                    RouteValues = new RouteValueDictionary()
                };

                menuItems.Add(categoryMenuItem);

                var eventTypes = _eventRegistry.FindEventTypesByCategory(category)
                                               .WhereIsDomainEvent();

                foreach (var eventType in eventTypes)
                {
                    var eventMenuItem = new EventMenuItem(eventType);
                    categoryMenuItem.Items.Add(eventMenuItem);
                }
            }

            // add resource rules menu item
            //menuItems.Add(new MenuItem()
            //{
            //    Name = "ResourceRules",
            //    Text = "Resource Rules",
            //    Area = "Commerce",
            //    Controller = "HAL",
            //    Action = "ResourceRules",
            //    RouteValues = new System.Web.Routing.RouteValueDictionary(),
            //    Initializer = new CommerceMenuItemInitializer()
            //});

            return menuItems;
        }
    }
}