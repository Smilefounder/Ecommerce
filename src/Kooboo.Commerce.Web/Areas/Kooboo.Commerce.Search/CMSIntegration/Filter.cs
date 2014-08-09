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
        public string Field { get; set; }

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

        public bool IsEmpty()
        {
            if (UseRangeFiltering)
            {
                return String.IsNullOrWhiteSpace(FromValue) && String.IsNullOrWhiteSpace(ToValue);
            }

            return String.IsNullOrWhiteSpace(FieldValue);
        }

        public bool IsMatch(FacetRange range)
        {
            if (!UseRangeFiltering)
            {
                return false;
            }

            if (FromInclusive != range.FromInclusive || ToInclusive != range.ToInclusive)
            {
                return false;
            }

            if (range.FromValue == null && !String.IsNullOrWhiteSpace(FromValue)
                || range.FromValue != null && String.IsNullOrWhiteSpace(FromValue))
            {
                return false;
            }

            if (range.ToValue == null && !String.IsNullOrWhiteSpace(ToValue)
                || range.ToValue != null && String.IsNullOrWhiteSpace(ToValue))
            {
                return false;
            }

            if (range.FromValue != null && Convert.ToDouble(FromValue) != range.FromValue.Value)
            {
                return false;
            }
            if (range.ToValue != null && Convert.ToDouble(ToValue) != range.ToValue.Value)
            {
                return false;
            }

            return true;
        }

        public Filter Parse(IValueProvider valueProvider)
        {
            var parsed = new Filter
            {
                Name = Name,
                Field = Field,
                UseRangeFiltering = UseRangeFiltering,
                FieldValue = ParameterizedFieldValue.GetFieldValue(FieldValue, valueProvider),
                FromValue = ParameterizedFieldValue.GetFieldValue(FromValue, valueProvider),
                FromInclusive = FromInclusive,
                ToValue = ParameterizedFieldValue.GetFieldValue(ToValue, valueProvider),
                ToInclusive = ToInclusive
            };

            return parsed;
        }
    }
}