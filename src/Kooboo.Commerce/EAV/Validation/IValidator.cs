using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Web.Mvc;

namespace Kooboo.Commerce.EAV.Validation
{
    public interface IValidator
    {
        string Name { get; }

        ValidationResult Validate(CustomField field, string value, FieldValidationRule rule);

        IEnumerable<ModelClientValidationRule> GetClientValidationRules(CustomField field, FieldValidationRule rule);
    }
}
