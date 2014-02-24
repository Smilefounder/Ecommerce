using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;

namespace Kooboo.Commerce.Activities
{
    public interface IActivityViews
    {
        string ActivityName { get; }

        RedirectToRouteResult Settings(ActivityBinding binding, ControllerContext controllerContext);
    }
}
