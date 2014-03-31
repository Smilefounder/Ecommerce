using Kooboo.CMS.Common.Runtime;
using Kooboo.CMS.Common.Runtime.Dependency;
using Kooboo.Commerce.Data;
using Kooboo.Commerce.Events.Dispatching;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Transactions;
using System.Web.Http.Filters;

namespace Kooboo.Commerce.Web.Http
{
    public class AutoDbCommitAttribute : ActionFilterAttribute
    {
        private ICommerceDatabase _database;

        public override void OnActionExecuting(System.Web.Http.Controllers.HttpActionContext actionContext)
        {
            base.OnActionExecuting(actionContext);

            var commerceContext = EngineContext.Current.Resolve<CommerceInstanceContext>();
            var currentInstance = commerceContext.CurrentInstance;
            if (currentInstance == null)
                throw new InvalidOperationException(typeof(AutoDbCommitAttribute).Name + " can only be applied to an action within commerce instance context.");

            _database = currentInstance.Database;
        }

        public override void OnActionExecuted(HttpActionExecutedContext actionExecutedContext)
        {
            try
            {
                if (actionExecutedContext.Exception == null)
                {
                    _database.SaveChanges();
                }
            }
            finally
            {
                // reset fields to null in case the filter instance is cached by asp.net mvc
                _database = null;
            }

            base.OnActionExecuted(actionExecutedContext);
        }
    }
}
