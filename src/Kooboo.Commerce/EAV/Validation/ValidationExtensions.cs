using Kooboo.CMS.Common.Runtime;
using Kooboo.Commerce.EAV.WebControls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace Kooboo.Commerce.EAV.Validation
{
    public static class ValidationExtensions
    {
        public static IEnumerable<ModelClientValidationRule> GetClientValidationRules(this CustomField field)
        {
            var rules = Enumerable.Empty<ModelClientValidationRule>();
            var validatorFactory = EngineContext.Current.Resolve<IValidatorFactory>();

            foreach (var rule in field.ValidationRules)
            {
                var validator = validatorFactory.FindByName(rule.ValidatorName);
                rules = rules.Union(validator.GetClientValidationRules(field, rule));
            }

            return rules;
        }

        public static IDictionary<string, object> GetUnobtrusiveValidationAtributes(this CustomField field)
        {
            var result = new Dictionary<string, object>();

            foreach (var rule in field.GetClientValidationRules())
            {
                var prefix = "data-val-" + rule.ValidationType;
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

        public static IHtmlString ValidationMessageFor(this HtmlHelper helper, WebControlModel controlModel)
        {
            var tagBuilder = new TagBuilder("span");
            tagBuilder.MergeAttribute("data-valmsg-for", controlModel.UniqueName);
            tagBuilder.MergeAttribute("data-valmsg-replace", "true");

            return new HtmlString(tagBuilder.ToString(TagRenderMode.Normal));
        }

        public static string GetUnobtrusiveValidationAttributeString(this CustomField field)
        {
            var sb = new StringBuilder();

            foreach (var attr in GetUnobtrusiveValidationAtributes(field))
            {
                sb.AppendFormat(" {0}=\"{1}\"", attr.Key, attr.Value);
            }

            return sb.ToString();
        }
    }
}
