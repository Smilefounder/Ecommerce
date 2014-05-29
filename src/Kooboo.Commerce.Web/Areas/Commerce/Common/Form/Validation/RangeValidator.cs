using Kooboo.CMS.Common.Runtime.Dependency;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Kooboo.Commerce.Web.Form.Validation
{
    [Dependency(typeof(IValidator), Key = "Range")]
    public class RangeValidator : IValidator
    {
        public string Name
        {
            get
            {
                return "Range";
            }
        }

        public IEnumerable<System.Web.Mvc.ModelClientValidationRule> GetClientValidationRules(EAV.CustomField field, EAV.FieldValidationRule rule)
        {
            if (!String.IsNullOrWhiteSpace(rule.ValidatorData))
            {
                var data = JsonConvert.DeserializeObject<RangeValidatorData>(rule.ValidatorData);
                yield return new ModelClientValidationRangeRule(rule.ErrorMessage, data.Start, data.End);
            }
        }
    }

    public class RangeValidatorData
    {
        public decimal Start { get; set; }

        public decimal End { get; set; }
    }
}