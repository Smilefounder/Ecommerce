using Kooboo.CMS.Common.Runtime;
using Kooboo.CMS.Common.Runtime.Dependency;
using Kooboo.Commerce.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;

namespace Kooboo.Commerce.Web.Framework.Mvc
{
    public class TransactionalAttribute : ActionFilterAttribute
    {
        const string _currentDbTransactionKey = "CurrentDbTransaction";

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            base.OnActionExecuting(filterContext);

            var instance = CommerceInstance.Current;
            if (instance == null)
                throw new InvalidOperationException(typeof(TransactionalAttribute).Name + " can only be applied to an action within commerce instance context.");

            filterContext.HttpContext.Items[_currentDbTransactionKey] = instance.Database.BeginTransaction();
        }

        public override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            var transaction = filterContext.HttpContext.Items[_currentDbTransactionKey] as ICommerceDbTransaction;

            try
            {
                if (filterContext.Exception == null && !transaction.IsCommitted)
                {
                    transaction.Commit();
                }
            }
            finally
            {
                transaction.Dispose();
            }

            base.OnActionExecuted(filterContext);
        }
    }
}
