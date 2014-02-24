using Kooboo.CMS.Common.Runtime.Dependency;
using Kooboo.Commerce.EAV.Validation;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Kooboo.Commerce.EAV.Validators
{
    [Dependency(typeof(IValidator), Key = "Kooboo.Commerce.EAV.Validators.RegexValidator")]
    public class RegexValidator : IValidator
    {
        public string Name
        {
            get
            {
                return "Regex";
            }
        }
        public ValidationResult Validate(CustomField field, string value, FieldValidationRule rule)
        {
            return ValidationResult.Success;
        }

        public IEnumerable<ModelClientValidationRule> GetClientValidationRules(CustomField field, FieldValidationRule rule)
        {
            var model = RegexValidatorData.Deserialize(rule.ValidatorData);
            yield return new ModelClientValidationRegexRule(rule.ErrorMessage, model.Pattern);
        }
    }

    public class RegexValidatorData
    {
        public string Pattern { get; set; }

        public static RegexValidatorData Deserialize(string data)
        {
            if (String.IsNullOrEmpty(data))
            {
                return new RegexValidatorData();
            }

            return JsonConvert.DeserializeObject<RegexValidatorData>(data);
        }
    }
}