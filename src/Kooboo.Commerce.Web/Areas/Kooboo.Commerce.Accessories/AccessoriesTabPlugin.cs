using Kooboo.Commerce.Products.Services;
using Kooboo.Commerce.Web.Framework.Tabs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Kooboo.Commerce.Accessories
{
    public class AccessoriesTabPlugin : TabPlugin<AccessoriesTabModel>
    {
        private IProductAccessoryService _accessoryService;

        public override string Name
        {
            get
            {
                return "Accessories";
            }
        }

        public override string VirtualPath
        {
            get
            {
                return "~/Areas/" + Strings.AreaName + "/Views/ProductAccessories.cshtml";
            }
        }

        public AccessoriesTabPlugin(IProductAccessoryService accessoryService)
        {
            _accessoryService = accessoryService;
        }

        public override bool IsVisible(System.Web.Mvc.ControllerContext controllerContext)
        {
            return controllerContext.AreaName() == "Commerce" 
                && controllerContext.ControllerName() == "Product" 
                && controllerContext.ActionName() == "Edit";
        }

        public override void OnLoad(TabLoadContext context)
        {
            var productId = Convert.ToInt32(context.Request.QueryString["id"]);

            var model = new AccessoriesTabModel
            {
                ProductId = productId,
                Accessories = _accessoryService.GetAccessories(productId).ToList()
            };

            context.Model = model;
        }

        public override void OnSubmit(TabSubmitContext<AccessoriesTabModel> context)
        {
            var model = context.Model;
            _accessoryService.UpdateAccessories(model.ProductId, model.Accessories);
        }
    }
}