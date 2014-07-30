using Kooboo.CMS.Common.Runtime;
using Kooboo.Commerce.Products;
using Kooboo.Commerce.Web.Framework.UI.Form.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace Kooboo.Commerce.Web.Framework.UI.Form
{
    public static class ValidationExtensions
    {
        public static IEnumerable<ModelClientValidationRule> GetClientValidationRules(this CustomFieldDefinition field)
        {
            var rules = Enumerable.Empty<ModelClientValidationRule>();
            var validators = ControlValidators.Validators().ToList();

            foreach (var rule in field.ValidationRules)
            {
                var validator = validators.Find(v => v.Name == rule.ValidatorName);
                if (validator != null)
                {
                    rules = rules.Union(validator.GetClientValidationRules(field, rule));
                }
            }

            return rules;
        }

        public static IDictionary<string, object> GetUnobtrusiveValidationAtributes(this CustomFieldDefinition field)
        {
            var result = new Dictionary<string, object>();

            foreach (var rule in field.GetClientValidationRules())
            {
                var prefix = "data-val-" + rule.ValidationType;
                if (result.ContainsKey(prefix))
                {
                    continue;
                }

                result.Add(prefix, rule.ErrorMessage ?? String.Empty);

                foreach (var kvp in rule.ValidationParameters)
                {
                    result.Add(prefix + "-" + kvp.Key, kvp.Value == null ? String.Empty : kvp.Value.ToString());
                }
            }

            if (result.Count > 0)
            {
                result.Add("data-val", "true");
            }

            return result;
        }
    }
}
