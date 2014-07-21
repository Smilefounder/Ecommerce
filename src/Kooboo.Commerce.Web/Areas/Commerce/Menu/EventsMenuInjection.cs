using Kooboo.Commerce.Rules.Activities;
using Kooboo.Commerce.Web.Framework.UI.Menu;
using Kooboo.Web.Mvc.Menu;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Routing;

namespace Kooboo.Commerce.Web.Areas.Commerce.Menu
{
    public class EventsMenuInjection : CommerceInstanceMenuInjection
    {
        public override void Inject(Kooboo.Web.Mvc.Menu.Menu menu, System.Web.Mvc.ControllerContext controllerContext)
        {
            var parent = menu.Items.FirstOrDefault(it => it.Name == "BusinessRules");

            parent.Items.Add(new MenuItem
            {
                Text = "Overview",
                Name = "Overview",
                Controller = "Rule",
                Area = "Commerce",
                Action = "Index",
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
                    Name = category
                };

                parent.Items.Add(categoryMenuItem);

                var events = manager.FindEvents(category).ToList();

                foreach (var each in events)
                {
                    var eventMenuItem = new MenuItem
                    {
                        Name = each.EventName,
                        Text = each.ShortName ?? each.DisplayName ?? each.EventName,
                        Controller = "Rule",
                        Action = "List",
                        Area = "Commerce",
                        RouteValues = new RouteValueDictionary(new { eventName = each.EventName }),
                        Initializer = new EventMenuItemInitializer()
                    };

                    categoryMenuItem.Items.Add(eventMenuItem);
                }
            }
        }
    }
}