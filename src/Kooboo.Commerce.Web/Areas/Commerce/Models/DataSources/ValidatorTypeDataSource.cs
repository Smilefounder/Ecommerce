using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Kooboo.CMS.Common.Runtime;
using Kooboo.Commerce.EAV.Validation;
using Kooboo.Globalization;
using Kooboo.Web.Mvc;

namespace Kooboo.Commerce.Web.Areas.Commerce.Models.DataSources {

    public class ValidatorTypeDataSource : ISelectListDataSource {

        private readonly IValidatorFactory _factory;
        public ValidatorTypeDataSource(IValidatorFactory factory) {
            _factory = factory;
        }

        public IEnumerable<SelectListItem> GetSelectListItems(RequestContext requestContext, string filter = null) {
            var validators = _factory.AllValidators();
            foreach (var item in validators) {
                yield return new SelectListItem() {
                    Text = item.Name,
                    Value = item.Name
                };
            }
        }
    }
}