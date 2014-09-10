using Kooboo.Commerce.Rules;
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

            var manager = EventSlotManager.Instance;

            foreach (var group in manager.GetGroups())
            {
                var groupItem = new MenuItem
                {
                    Text = group,
                    Name = group
                };

                parent.Items.Add(groupItem);

                var slots = manager.GetSlots(group).ToList();

                foreach (var each in slots)
                {
                    var eventItem = new MenuItem
                    {
                        Name = each.EventType.Name,
                        Text = each.ShortName ?? each.EventType.Name,
                        Controller = "Rule",
                        Action = "List",
                        Area = "Commerce",
                        RouteValues = new RouteValueDictionary(new { eventName = each.EventType.Name }),
                        Initializer = new EventMenuItemInitializer()
                    };

                    groupItem.Items.Add(eventItem);
                }
            }
        }
    }
}