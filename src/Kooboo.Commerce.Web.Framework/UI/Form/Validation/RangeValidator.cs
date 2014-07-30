using Kooboo.CMS.Common.Runtime.Dependency;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Kooboo.Commerce.Web.Framework.UI.Form.Validation
{
    [Dependency(typeof(IValidator), Key = "Range", Order = 200)]
    public class RangeValidator : IValidator
    {
        public string Name
        {
            get
            {
                return "Range";
            }
        }

        public IEnumerable<System.Web.Mvc.ModelClientValidationRule> GetClientValidationRules(Products.CustomFieldDefinition field, Products.FieldValidationRule rule)
        {
            if (!String.IsNullOrWhiteSpace(rule.ValidatorConfig))
            {
                var data = JsonConvert.DeserializeObject<RangeValidatorConfig>(rule.ValidatorConfig);
                yield return new ModelClientValidationRangeRule(rule.ErrorMessage, data.MinValue, data.MaxValue);
            }
        }
    }

    public class RangeValidatorConfig
    {
        public decimal MinValue { get; set; }

        public decimal MaxValue { get; set; }
    }
}