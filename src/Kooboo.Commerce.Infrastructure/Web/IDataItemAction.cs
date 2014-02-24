using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;

namespace Kooboo.Commerce.Web
{
    public interface IDataItemAction<T>
    {
        string Name { get; }

        ActionResult Execute(T dataItem, ControllerContext controllerContext);
    }

    public class DataItemAction<T> : IDataItemAction<T>
    {
        public string Name { get; private set; }

        public ActionResult ActionResult { get; private set; }

        public DataItemAction(string name, ActionResult actionResult)
        {
            Name = name;
            ActionResult = actionResult;
        }

        public ActionResult Execute(T dataItem, ControllerContext controllerContext)
        {
            return ActionResult;
        }
    }
}
