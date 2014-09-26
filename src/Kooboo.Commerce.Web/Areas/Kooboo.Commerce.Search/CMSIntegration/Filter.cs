using Kooboo.CMS.Sites.DataRule;
using Kooboo.Commerce.Search.Facets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;
using System.Web.Mvc;

namespace Kooboo.Commerce.Search.CMSIntegration
{
    [DataContract]
    [KnownType(typeof(Filter))]
    public class Filter
    {
        [DataMember]
        public string Name { get; set; }

        [DataMember]
        public string Fields { get; set; }

        [DataMember]
        public bool UseRangeFiltering { get; set; }

        [DataMember]
        public string FieldValue { get; set; }

        [DataMember]
        public string FromValue { get; set; }

        [DataMember]
        public bool FromInclusive { get; set; }

        [DataMember]
        public string ToValue { get; set; }

        [DataMember]
        public bool ToInclusive { get; set; }

        [DataMember]
        public bool AnalyzeInput { get; set; }

        public Filter Parse(IValueProvider valueProvider)
        {
            var parsed = new Filter
            {
                Name = Name,
                Fields = Fields,
                UseRangeFiltering = UseRangeFiltering,
                FieldValue = ParameterizedFieldValue.GetFieldValue(FieldValue, valueProvider),
                FromValue = ParameterizedFieldValue.GetFieldValue(FromValue, valueProvider),
                FromInclusive = FromInclusive,
                ToValue = ParameterizedFieldValue.GetFieldValue(ToValue, valueProvider),
                ToInclusive = ToInclusive,
                AnalyzeInput = AnalyzeInput
            };

            return parsed;
        }

        public string[] GetFields()
        {
            return Fields.Split(',');
        }
    }
}