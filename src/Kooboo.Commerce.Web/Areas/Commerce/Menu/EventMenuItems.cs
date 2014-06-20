﻿using Kooboo.CMS.Common.Runtime;
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
        public EventMenuItem(EventRegistrationEntry entry)
        {
            Name = entry.EventType.Name;
            Text = entry.ShortName ?? entry.DisplayName ?? entry.EventType.Name.Humanize();

            Controller = "Activity";
            Action = "List";
            Area = "Commerce";

            RouteValues = new System.Web.Routing.RouteValueDictionary();
            RouteValues.Add("eventType", entry.EventType.AssemblyQualifiedNameWithoutVersion());

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

            menuItems.Add(new MenuItem
            {
                Text = "Overview",
                Name = "Overview",
                RouteValues = new RouteValueDictionary(),
                Controller = "Activity",
                Area = "Commerce",
                Action = "Index",
                Initializer = new CommerceMenuItemInitializer(),
                ReadOnlyProperties = new System.Collections.Specialized.NameValueCollection
                {
                    { "activeByAction", "true" }
                }
            });

            foreach (var category in _eventRegistry.AllCategories())
            {
                var categoryMenuItem = new MenuItem
                {
                    Text = category.Name,
                    Name = category.Name,
                    RouteValues = new RouteValueDictionary()
                };

                menuItems.Add(categoryMenuItem);

                var eventEntries = _eventRegistry.FindByCategory(category.Name)
                                                 .Where(e => e.EventType.IsBusinessEvent());

                foreach (var entry in eventEntries)
                {
                    var eventMenuItem = new EventMenuItem(entry);
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