using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Kooboo.Globalization;
using Kooboo.Web.Mvc;

namespace Kooboo.Commerce.Web.Areas.Commerce.Models.DataSources {

    public class CultureDataSource : ISelectListDataSource {

        public IEnumerable<SelectListItem> GetSelectListItems(RequestContext requestContext, string filter = null) {
            foreach (var item in CultureInfo.GetCultures(CultureTypes.AllCultures).OrderBy(i => i.DisplayName)) {
                yield return new SelectListItem { Text = item.DisplayName, Value = item.Name };
            }
        }
    }
}