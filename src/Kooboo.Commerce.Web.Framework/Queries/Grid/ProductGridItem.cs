using Kooboo.Commerce.Events;
using Kooboo.Commerce.Events.Products;
using Kooboo.Web.Mvc.Grid2;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;

namespace Kooboo.Commerce.Web.Framework.Queries.Grid
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

            var @event = new GetProductActions(model.Id);
            Event.Raise(@event);

            var classes = new List<string>();

            foreach (var actionName in @event.ActionNames)
            {
                classes.Add("action-" + actionName);
            }

            return MvcHtmlString.Create(String.Format("class='{0}'", String.Join(" ", classes)));
        }
    }
}
