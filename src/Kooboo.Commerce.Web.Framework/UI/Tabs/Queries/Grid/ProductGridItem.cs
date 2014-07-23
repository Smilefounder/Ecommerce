using Kooboo.Commerce.Data;
using Kooboo.Commerce.Events;
using Kooboo.Commerce.Events.Products;
using Kooboo.Commerce.Products;
using Kooboo.Commerce.Web.Framework.UI.Topbar;
using Kooboo.Web.Mvc.Grid2;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;

namespace Kooboo.Commerce.Web.Framework.UI.Tabs.Queries.Grid
{
    public class ProductGridItem : GridItem
    {
        public ProductGridItem(IGridModel gridModel, object dataItem, int dataIndex)
            : base(gridModel, dataItem, dataIndex)
        {
        }

        public override System.Web.IHtmlString RenderItemContainerAtts()
        {
            var model = DataItem as IProductModel;
            if (model == null)
            {
                return MvcHtmlString.Empty;
            }

            var instance = CommerceInstance.Current;
            var product = instance.Database.GetRepository<Product>().Find(model.Id);

            var classes = new List<string>();
            foreach (var button in TopbarCommands.GetCommands(GridModel.ViewContext.Controller.ControllerContext, product, instance))
            {
                classes.Add("cmd-" + button.Name);
            }

            return MvcHtmlString.Create(String.Format("class='{0}'", String.Join(" ", classes)));
        }
    }
}
