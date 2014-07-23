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
using Kooboo.Commerce.Web.Framework.UI.Form.Validation;

namespace Kooboo.Commerce.Web.Areas.Commerce.Models.DataSources
{
    public class ValidatorTypeDataSource : ISelectListDataSource
    {
        public IEnumerable<SelectListItem> GetSelectListItems(RequestContext requestContext, string filter = null)
        {
            var validators = ControlValidators.Validators();
            foreach (var item in validators)
            {
                yield return new SelectListItem()
                {
                    Text = item.Name,
                    Value = item.Name
                };
            }
        }
    }
}