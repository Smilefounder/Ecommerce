using Lucene.Net.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Kooboo.Commerce.Search.Facets
{
    class EncodedFacetRange
    {
        public string Label { get; set; }

        public string FromValue { get; set; }

        public bool FromInclusive { get; set; }

        public string ToValue { get; set; }

        public bool ToInclusive { get; set; }

        /// <summary>
        /// Check if this range includes the specified value.
        /// </summary>
        public bool Includes(string value)
        {
            if (FromValue != null && String.CompareOrdinal(value, FromValue) < 0)
            {
                return false;
            }
            if (ToValue != null && String.CompareOrdinal(value, ToValue) > 0)
            {
                return false;
            }

            return true;
        }

        public static EncodedFacetRange Encode(FacetRange range)
        {
            var result = new EncodedFacetRange
            {
                Label = range.Label,
                FromInclusive = range.FromInclusive,
                ToInclusive = range.ToInclusive
            };

            if (range.FromValue != null)
            {
                result.FromValue = NumericUtils.DoubleToPrefixCoded(range.FromValue.Value);
                //result.FromValue = LuceneUtil.Unescape(result.FromValue);
            }
            if (range.ToValue != null)
            {
                result.ToValue = NumericUtils.DoubleToPrefixCoded(range.ToValue.Value);
                //result.ToValue = LuceneUtil.Unescape(result.ToValue);
            }

            return result;
        }
    }
}