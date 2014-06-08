using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;

namespace Kooboo.Commerce.Activities.OrderReminder
{
    public static class Template
    {
        static readonly Regex _paramPattern = new Regex(@"\{[\w_]+\}", RegexOptions.Compiled | RegexOptions.IgnoreCase);

        public static string Render(string template, object model)
        {
            if (String.IsNullOrWhiteSpace(template) || model == null)
            {
                return template;
            }

            var modelType = model.GetType();

            return _paramPattern.Replace(template, match =>
            {
                var param = match.Value;
                var prop = modelType.GetProperty(param, System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance);
                if (prop != null)
                {
                    var paramValue = prop.GetValue(model, null);
                    if (paramValue == null)
                    {
                        return String.Empty;
                    }

                    return paramValue.ToString();
                }

                return match.Value;
            });
        }
    }
}