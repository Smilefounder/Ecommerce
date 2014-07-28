using Kooboo.CMS.Common.Runtime.Dependency;
using Kooboo.Commerce.Products;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Kooboo.Commerce.Web.Framework.UI.Form.Validation
{
    [Dependency(typeof(IValidator), Key = "Required", Order = 100)]
    public class RequiredValidator : IValidator
    {
        public string Name
        {
            get
            {
                return "Required";
            }
        }

        public IEnumerable<System.Web.Mvc.ModelClientValidationRule> GetClientValidationRules(CustomField field, FieldValidationRule rule)
        {
            yield return new ModelClientValidationRequiredRule(rule.ErrorMessage);
        }
    }
}