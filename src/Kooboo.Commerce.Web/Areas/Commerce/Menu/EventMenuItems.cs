using Kooboo.CMS.Common.Runtime;
using Kooboo.Commerce.Activities;
using Kooboo.Commerce.Events;
using Kooboo.Web.Mvc.Menu;
using Kooboo.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Routing;
using Kooboo.Commerce.Rules.Activities;

namespace Kooboo.Commerce.Web.Areas.Commerce.Menu
{
    public class EventMenuItem : MenuItem
    {
        public EventMenuItem(EventEntry entry)
        {
            Name = entry.EventType.Name;
            Text = entry.ShortName ?? entry.DisplayName ?? entry.EventType.Name;

            Controller = "Rule";
            Action = "List";
            Area = "Commerce";

            RouteValues = new System.Web.Routing.RouteValueDictionary();
            RouteValues.Add("eventName", entry.EventName);

            Initializer = new EventMenuItemInitializer();
        }
    }

    public class EventMenuItems : IMenuItemContainer
    {
        public IEnumerable<MenuItem> GetItems(string areaName, System.Web.Mvc.ControllerContext controllerContext)
        {
            var menuItems = new List<MenuItem>();

            menuItems.Add(new MenuItem
            {
                Text = "Overview",
                Name = "Overview",
                RouteValues = new RouteValueDictionary(),
                Controller = "Rule",
                Area = "Commerce",
                Action = "Index",
                Initializer = new CommerceMenuItemInitializer(),
                ReadOnlyProperties = new System.Collections.Specialized.NameValueCollection
                {
                    { "activeByAction", "true" }
                }
            });

            var manager = ActivityEventManager.Instance;

            foreach (var category in manager.Categories)
            {
                var categoryMenuItem = new MenuItem
                {
                    Text = category,
                    Name = category,
                    RouteValues = new RouteValueDictionary()
                };

                menuItems.Add(categoryMenuItem);

                var events = manager.FindEvents(category).ToList();

                foreach (var each in events)
                {
                    var eventMenuItem = new EventMenuItem(each);
                    categoryMenuItem.Items.Add(eventMenuItem);
                }
            }

            return menuItems;
        }
    }
}