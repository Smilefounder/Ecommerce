using Kooboo.CMS.Common;
using Kooboo.CMS.Common.Runtime;
using Kooboo.CMS.Common.Runtime.Dependency;
using Kooboo.CMS.Common.Runtime.Mvc;
using Kooboo.Commerce.Activities.Jobs;
using Kooboo.Commerce.ComponentModel.DataAnnotations;
using Kooboo.Commerce.Events;
using Kooboo.Commerce.Events.Dispatching;
using Kooboo.Commerce.Web.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Compilation;
using System.Web.Mvc;

namespace Kooboo.Commerce.Web.Areas.Commerce
{
    [Dependency(typeof(IHttpApplicationEvents), Key = "CommerceApplicationEvents")]
    public class CommerceApplicationEvents : Kooboo.CMS.Common.Runtime.Mvc.MvcModule
    {
        public override void Application_Start(object sender, EventArgs e)
        {
            base.Application_Start(sender, e);

            // Mvc
            DataAnnotationsModelValidatorProvider.RegisterAdapter(
                typeof(RequiredIfAttribute), typeof(RequiredAttributeAdapter));

            // Async Activity
            Kooboo.Job.Jobs.Instance.AttachJob(typeof(AsyncActivityExecutionJob).FullName, new AsyncActivityExecutionJob(), 30 * 1000, null);
        }
    }
}