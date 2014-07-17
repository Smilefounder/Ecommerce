using Kooboo.Commerce.Customers;
using Kooboo.Commerce.Data;
using Kooboo.Commerce.Events;
using Kooboo.Commerce.Web.Framework.UI.Toolbar;
using Kooboo.Web.Mvc.Grid2;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;

namespace Kooboo.Commerce.Web.Framework.Queries.Grid
{
    public class CustomerGridItem : GridItem
    {
        public CustomerGridItem(IGridModel gridModel, object dataItem, int dataIndex)
            : base(gridModel, dataItem, dataIndex)
        {
        }

        public override System.Web.IHtmlString RenderItemContainerAtts()
        {
            var model = DataItem as ICustomerModel;
            if (model == null)
            {
                return MvcHtmlString.Empty;
            }

            var instance = CommerceInstance.Current;
            var customer = instance.Database.GetRepository<Customer>().Find(model.Id);

            var classes = new List<string>();
            foreach (var button in ToolbarCommands.GetCommands(GridModel.ViewContext.Controller.ControllerContext, customer, instance))
            {
                classes.Add("cmd-" + button.Name);
            }

            return MvcHtmlString.Create(String.Format("class='{0}'", String.Join(" ", classes)));
        }
    }
}
