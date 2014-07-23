using Kooboo.CMS.Common.Runtime.Dependency;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Kooboo.Commerce.Web.Framework.UI.Form.Validation
{
    [Dependency(typeof(IValidator), Key = "StringLength")]
    public class StringLengthValidator : IValidator
    {
        public string Name
        {
            get
            {
                return "StringLength";
            }
        }

        public IEnumerable<System.Web.Mvc.ModelClientValidationRule> GetClientValidationRules(EAV.CustomField field, EAV.FieldValidationRule rule)
        {
            if (!String.IsNullOrWhiteSpace(rule.ValidatorData))
            {
                var data = JsonConvert.DeserializeObject<StringLengthValidatorData>(rule.ValidatorData);
                yield return new ModelClientValidationStringLengthRule(rule.ErrorMessage, data.Min, data.Max);
            }
        }
    }

    public class StringLengthValidatorData
    {
        public int Min { get; set; }

        public int Max { get; set; }
    }
}