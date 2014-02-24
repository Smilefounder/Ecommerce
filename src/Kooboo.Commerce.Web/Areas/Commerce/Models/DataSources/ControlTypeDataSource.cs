using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Kooboo.CMS.Common.Runtime;
using Kooboo.Commerce.EAV.WebControls;
using Kooboo.Globalization;
using Kooboo.Web.Mvc;

namespace Kooboo.Commerce.Web.Areas.Commerce.Models.DataSources {

    public class ControlTypeDataSource : ISelectListDataSource {

        private readonly IWebControlFactory _factory;
        public ControlTypeDataSource(IWebControlFactory factory) {
            _factory = factory;
        }

        public IEnumerable<SelectListItem> GetSelectListItems(RequestContext requestContext, string filter = null) {
            var controls = _factory.AllWebControls();
            foreach (var item in controls) {
                yield return new SelectListItem() {
                    Text = item.Name,
                    Value = item.Name
                };
            }
        }
    }
}