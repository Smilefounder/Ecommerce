using Kooboo.CMS.Sites.DataRule;
using Kooboo.Commerce.Api.Metadata;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace Kooboo.Commerce.CMSIntegration.DataSources.Generic
{
    [DataContract]
    public class SavedFilter
    {
        [DataMember]
        public string Name { get; set; }

        [DataMember]
        public bool Required { get; set; }

        [DataMember]
        public List<SavedFilterParameterValue> ParameterValues { get; set; }

        public SavedFilter()
        {
            ParameterValues = new List<SavedFilterParameterValue>();
        }

        public ParsedFilter Parse(FilterDescription definition, CommerceDataSourceContext context)
        {
            var paramValues = new Dictionary<string, object>(StringComparer.OrdinalIgnoreCase);

            foreach (var paramDef in definition.Parameters)
            {
                var param = ParameterValues.Find(p => p.ParameterName.Equals(paramDef.Name, StringComparison.OrdinalIgnoreCase));

                // Parameter is required but it's missing in data source settings (might cause by code changes). Parse failed.
                if (paramDef.Required && param == null)
                {
                    return null;
                }

                var strParamValue = ParameterizedFieldValue.GetFieldValue(param.ParameterValue, context.ValueProvider);
                // Parameter is required but it's missing in current context. Parse failed.
                if (paramDef.Required && strParamValue == null)
                {
                    return null;
                }

                var paramValue = ResolveParameterValue(strParamValue, paramDef.ValueType);
                paramValues.Add(paramDef.Name, paramValue);
            }

            return new ParsedFilter(definition.Name)
            {
                ParameterValues = paramValues
            };
        }

        private object ResolveParameterValue(string strValue, Type type)
        {
            if (String.IsNullOrWhiteSpace(strValue))
            {
                return null;
            }

            Type targetType = type;

            if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>))
            {
                targetType = Nullable.GetUnderlyingType(type);
            }

            return Convert.ChangeType(strValue, targetType);
        }
    }
}