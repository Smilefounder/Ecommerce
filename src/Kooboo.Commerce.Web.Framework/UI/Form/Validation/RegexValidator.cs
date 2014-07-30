using Kooboo.CMS.Common.Runtime.Dependency;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Kooboo.Commerce.Web.Framework.UI.Form.Validation
{
    [Dependency(typeof(IValidator), Key = "Regex", Order = 300)]
    public class RegexValidator : IValidator
    {
        public string Name
        {
            get
            {
                return "Regex";
            }
        }

        public IEnumerable<System.Web.Mvc.ModelClientValidationRule> GetClientValidationRules(Products.CustomFieldDefinition field, Products.FieldValidationRule rule)
        {
            if (!String.IsNullOrWhiteSpace(rule.ValidatorConfig))
            {
                var data = JsonConvert.DeserializeObject<RegexValidatorConfig>(rule.ValidatorConfig);
                yield return new ModelClientValidationRegexRule(rule.ErrorMessage, data.Pattern);
            }
        }
    }

    public class RegexValidatorConfig
    {
        public string Pattern { get; set; }
    }
}