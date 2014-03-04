using Kooboo.CMS.Common.Runtime.Dependency;
using Kooboo.Commerce.Data;
using Kooboo.Commerce.Events.Dispatching;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;

namespace Kooboo.Commerce.Web.Mvc
{
    public class TransactionalAttribute : ActionFilterAttribute
    {
        private CommerceTransactionScope _transactionScope;

        [Inject]
        public CommerceInstanceContext CommerceContext { get; set; }

        [Inject]
        public IEventDispatcher EventDispatcher { get; set; }

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            base.OnActionExecuting(filterContext);

            _transactionScope = new CommerceTransactionScope(EventDispatcher);
        }

        public override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            try
            {
                if (filterContext.Exception == null)
                {
                    var currentCommerceInstance = CommerceContext.CurrentInstance;
                    if (currentCommerceInstance != null)
                    {
                        currentCommerceInstance.Database.SaveChanges();
                    }

                    _transactionScope.Complete();
                }
            }
            finally
            {
                _transactionScope.Dispose();
                _transactionScope = null;
            }

            base.OnActionExecuted(filterContext);
        }
    }
}
