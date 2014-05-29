using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Kooboo.CMS.Common.Runtime;
using Kooboo.Globalization;
using Kooboo.Web.Mvc;
using Kooboo.Commerce.Web.Form;

namespace Kooboo.Commerce.Web.Areas.Commerce.Models.DataSources
{

    public class ControlTypeDataSource : ISelectListDataSource
    {
        public IEnumerable<SelectListItem> GetSelectListItems(RequestContext requestContext, string filter = null)
        {
            foreach (var item in FormControls.Controls())
            {
                yield return new SelectListItem
                {
                    Text = item.Name,
                    Value = item.Name
                };
            }
        }
    }
}