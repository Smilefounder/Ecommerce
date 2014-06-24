using Kooboo.CMS.Common.Runtime.Dependency;
using Kooboo.Commerce.EAV;
using Kooboo.Commerce.Web.Form.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Kooboo.Commerce.Web.Areas.Commerce.Common.Form.Validation
{
    [Dependency(typeof(IValidator), Key = "Unique")]
    public class UniqueValidator : IValidator
    {
        public string Name
        {
            get
            {
                return "Unique";
            }
        }

        public IEnumerable<System.Web.Mvc.ModelClientValidationRule> GetClientValidationRules(CustomField field, FieldValidationRule rule)
        {
            yield return new ModelClientValidationRule
            {
                ValidationType = "uniquefield",
                ErrorMessage = rule.ErrorMessage
            };
        }
    }
}