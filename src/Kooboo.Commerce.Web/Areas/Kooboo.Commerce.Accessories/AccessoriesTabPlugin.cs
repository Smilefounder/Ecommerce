using Kooboo.Commerce.Products;
using Kooboo.Commerce.Web.Framework.UI;
using Kooboo.Commerce.Web.Framework.UI.Tabs;
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

        public override IEnumerable<Web.Framework.UI.MvcRoute> ApplyTo
        {
            get
            {
                yield return MvcRoutes.Products.Edit();
            }
        }

        public AccessoriesTabPlugin(IProductAccessoryService accessoryService)
        {
            _accessoryService = accessoryService;
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