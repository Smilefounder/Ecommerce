using Kooboo.CMS.Common.Runtime.Dependency;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Kooboo.Commerce.Web.Form.Validation
{
    [Dependency(typeof(IValidator), Key = "Regex")]
    public class RegexValidator : IValidator
    {
        public string Name
        {
            get
            {
                return "Regex";
            }
        }

        public IEnumerable<System.Web.Mvc.ModelClientValidationRule> GetClientValidationRules(EAV.CustomField field, EAV.FieldValidationRule rule)
        {
            if (!String.IsNullOrWhiteSpace(rule.ValidatorData))
            {
                var data = JsonConvert.DeserializeObject<RegexValidatorData>(rule.ValidatorData);
                yield return new ModelClientValidationRegexRule(rule.ErrorMessage, data.Pattern);
            }
        }
    }

    public class RegexValidatorData
    {
        public string Pattern { get; set; }
    }
}