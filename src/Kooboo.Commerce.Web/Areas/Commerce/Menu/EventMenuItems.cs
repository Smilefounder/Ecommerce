using Kooboo.CMS.Common.Runtime;
using Kooboo.Commerce.Activities;
using Kooboo.Commerce.Events;
using Kooboo.Commerce.Events.Registry;
using Kooboo.Web.Mvc.Menu;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Kooboo.Commerce.Web.Areas.Commerce.Menu
{
    public class EventCategoryMenuItem : MenuItem
    {
        public string Category { get; set; }

        public EventCategoryMenuItem(string category)
        {
            Category = category;

            Name = category;
            Text = category;

            Controller = "Activity";
            Action = "Events";

            RouteValues = new System.Web.Routing.RouteValueDictionary();
            RouteValues.Add("category", category);

            Initializer = new EventCategoryMenuItemInitializer();
        }
    }

    public class EventMenuItem : MenuItem
    {
        public EventMenuItem(Type eventType)
        {
            Name = eventType.Name;
            Text = eventType.GetDescription() ?? Name;

            Controller = "Activity";
            Action = "List";

            RouteValues = new System.Web.Routing.RouteValueDictionary();
            RouteValues.Add("eventType", eventType.GetVersionUnawareAssemblyQualifiedName());

            Initializer = new EventMenuItemInitializer();
        }
    }

    public class EventMenuItems : IMenuItemContainer
    {
        private IActivityEventRegistry _eventRegistry;

        public EventMenuItems()
            : this(EngineContext.Current.Resolve<IActivityEventRegistry>())
        {
        }

        public EventMenuItems(IActivityEventRegistry eventRegistry)
        {
            Require.NotNull(eventRegistry, "eventRegistry");
            _eventRegistry = eventRegistry;
        }

        public IEnumerable<MenuItem> GetItems(string areaName, System.Web.Mvc.ControllerContext controllerContext)
        {
            var menuItems = new List<MenuItem>();

            foreach (var category in _eventRegistry.GetCategories())
            {
                var categoryMenuItem = new EventCategoryMenuItem(category);
                menuItems.Add(categoryMenuItem);

                var eventTypes = _eventRegistry.GetEventTypesByCategory(category);

                foreach (var eventType in eventTypes)
                {
                    var eventMenuItem = new EventMenuItem(eventType);
                    categoryMenuItem.Items.Add(eventMenuItem);
                }
            }

            return menuItems;
        }
    }
}