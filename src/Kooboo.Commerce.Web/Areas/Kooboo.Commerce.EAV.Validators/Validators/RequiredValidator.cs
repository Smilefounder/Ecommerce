using Kooboo.CMS.Common.Runtime.Dependency;
using Kooboo.Commerce.EAV.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Kooboo.Commerce.EAV.Validators
{
    [Dependency(typeof(IValidator), Key = "Kooboo.Commerce.EAV.Validators.RequiredValidator")]
    public class RequiredValidator : IValidator
    {
        public string Name
        {
            get
            {
                return "Required";
            }
        }

        public ValidationResult Validate(CustomField field, string value, FieldValidationRule rule)
        {
            return ValidationResult.Success;
        }

        public IEnumerable<System.Web.Mvc.ModelClientValidationRule> GetClientValidationRules(CustomField field, FieldValidationRule rule)
        {
            yield return new ModelClientValidationRequiredRule(rule.ErrorMessage);
        }
    }
}