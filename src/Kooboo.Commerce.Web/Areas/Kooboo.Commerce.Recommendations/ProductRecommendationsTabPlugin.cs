using Kooboo.Commerce.Web.Framework.Tabs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Kooboo.Commerce.Recommendations
{
    public class ProductRecommendationsTabPlugin : TabPlugin<RecommendationsTabModel>
    {
        private IProductRecommendationService _service;

        public override string Name
        {
            get
            {
                return "Recommendations";
            }
        }

        public override string VirtualPath
        {
            get
            {
                return "~/Areas/" + Strings.AreaName + "/Views/Recommendations.cshtml";
            }
        }

        public ProductRecommendationsTabPlugin(IProductRecommendationService service)
        {
            _service = service;
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
            var recommendations = _service.GetRecommendations(productId).ToList();

            context.Model = new RecommendationsTabModel
            {
                ProductId = productId,
                Recommendations = recommendations
            };
        }

        public override void OnSubmit(TabSubmitContext<RecommendationsTabModel> context)
        {
            _service.SaveRecommendations(context.Model.ProductId, context.Model.Recommendations);
        }
    }
}