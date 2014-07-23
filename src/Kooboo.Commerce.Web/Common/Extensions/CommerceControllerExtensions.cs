using Kooboo.Commerce.Web.Areas.Commerce.Models.TabQueries;
using Kooboo.Commerce.Web.Framework.Mvc;
using Kooboo.Commerce.Web.Framework.UI.Tabs;
using Kooboo.Commerce.Web.Framework.UI.Tabs.Queries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Kooboo.Commerce.Web
{
    static class CommerceControllerExtensions
    {
        public static IEnumerable<LoadedTabPlugin> LoadTabPlugins(this CommerceController controller)
        {
            var plugins = TabPlugins.LoadTabPlugins(controller.ControllerContext);
            controller.ViewData["LoadedTabPlugins"] = plugins;
            return plugins;
        }

        public static TabQueryModel CreateTabQueryModel(this CommerceController controller, string pageName, ITabQuery defaultQuery, string defaultQueryDisplayName = "All")
        {
            var manager = new SavedTabQueryManager();

            var model = new TabQueryModel
            {
                PageName = pageName,
                SavedQueries = manager.FindAll(pageName).ToList(),
                AvailableQueries = TabQueries.GetQueries(controller.ControllerContext).ToList()
            };

            // Ensure default
            if (model.SavedQueries.Count == 0)
            {
                var savedQuery = SavedTabQuery.CreateFrom(defaultQuery, "All");
                manager.Add(model.PageName, savedQuery);
                model.SavedQueries.Add(savedQuery);
            }

            var queryId = controller.Request.QueryString["queryId"];

            if (String.IsNullOrEmpty(queryId))
            {
                model.CurrentQuery = model.SavedQueries.FirstOrDefault();
            }
            else
            {
                model.CurrentQuery = manager.Find(model.PageName, new Guid(queryId));
            }

            var query = model.AvailableQueries.Find(q => q.Name == model.CurrentQuery.QueryName);

            var search = controller.Request.QueryString["search"];
            var page = 1;
            if (!String.IsNullOrEmpty(controller.Request.QueryString["page"]))
            {
                page = Convert.ToInt32(controller.Request.QueryString["page"]);
            }

            var pageSize = 50;
            if (!String.IsNullOrEmpty(controller.Request.QueryString["pageSize"]))
            {
                pageSize = Convert.ToInt32(controller.Request.QueryString["pageSize"]);
            }

            model.CurrentQueryResult = query.Execute(new QueryContext(controller.CurrentInstance, search, page - 1, pageSize, model.CurrentQuery.Config))
                                            .ToPagedList();

            return model;
        }
    }
}