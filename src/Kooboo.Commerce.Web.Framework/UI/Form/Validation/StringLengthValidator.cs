using Kooboo.CMS.Common.Runtime.Dependency;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Kooboo.Commerce.Web.Framework.UI.Form.Validation
{
    [Dependency(typeof(IValidator), Key = "StringLength", Order = 400)]
    public class StringLengthValidator : IValidator
    {
        public string Name
        {
            get
            {
                return "StringLength";
            }
        }

        public IEnumerable<System.Web.Mvc.ModelClientValidationRule> GetClientValidationRules(Products.CustomField field, Products.FieldValidationRule rule)
        {
            if (!String.IsNullOrWhiteSpace(rule.ValidatorConfig))
            {
                var data = JsonConvert.DeserializeObject<StringLengthValidatorConfig>(rule.ValidatorConfig);
                yield return new ModelClientValidationStringLengthRule(rule.ErrorMessage, data.MinLength, data.MaxLength);
            }
        }
    }

    public class StringLengthValidatorConfig
    {
        public int MinLength { get; set; }

        public int MaxLength { get; set; }
    }
}