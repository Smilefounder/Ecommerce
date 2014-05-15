using Kooboo.CMS.Sites.DataRule;
using Kooboo.CMS.Sites.DataSource;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace Kooboo.Commerce.CMSIntegration.DataSources
{
    [DataContract(Name = "CommerceDataSource")]
    [KnownType(typeof(CommerceDataSource))]
    public class CommerceDataSource : IDataSource
    {
        [DataMember]
        public string ResourceName { get; set; }

        [DataMember]
        public Dictionary<string, string> RequiredParameterValues { get; set; }

        [DataMember]
        public Dictionary<string, string> OptionalParameterValues { get; set; }

        public CommerceDataSource()
        {
            RequiredParameterValues = new Dictionary<string, string>();
            OptionalParameterValues = new Dictionary<string, string>();
        }

        public object Execute(DataSourceContext dataSourceContext)
        {
            var paramValues = PopulateParameterValues(dataSourceContext);

            return null;
        }

        private Dictionary<string, string> PopulateParameterValues(DataSourceContext context)
        {
            var paramValues = new Dictionary<string, string>();

            foreach (var each in RequiredParameterValues.Union(OptionalParameterValues).ToList())
            {
                if (!String.IsNullOrEmpty(each.Value))
                {
                    var value = ParameterizedFieldValue.GetFieldValue(each.Value, context.ValueProvider);
                    paramValues.Add(each.Key, value);
                }
            }

            return paramValues;
        }

        public bool HasAnyParameters()
        {
            if (RequiredParameterValues.Values.Any(v => !String.IsNullOrEmpty(v) && ParameterizedFieldValue.IsParameterizedField(v)))
            {
                return true;
            }

            if (OptionalParameterValues.Values.Any(v => !String.IsNullOrEmpty(v) && ParameterizedFieldValue.IsParameterizedField(v)))
            {
                return true;
            }

            return false;
        }
    }
}