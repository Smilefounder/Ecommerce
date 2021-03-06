﻿using Lucene.Net.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Web;

namespace Kooboo.Commerce.Search.Facets
{
    [DataContract]
    [KnownType(typeof(FacetRange))]
    public class FacetRange
    {
        [DataMember]
        public string Label { get; set; }

        [DataMember]
        public bool FromInclusive { get; set; }

        [DataMember]
        public double? FromValue { get; set; }

        [DataMember]
        public bool ToInclusive { get; set; }

        [DataMember]
        public double? ToValue { get; set; }

        public FacetRange() { }

        public FacetRange(string label)
        {
            Label = label;
        }

        public override string ToString()
        {
            var exp = new StringBuilder();
            exp.Append(FromInclusive ? "[" : "{");
            exp.Append(FromValue == null ? "*" : FromValue.Value.ToString());
            exp.Append(" TO ");
            exp.Append(ToValue == null ? "*" : ToValue.Value.ToString());
            exp.Append(ToInclusive ? "]" : "}");

            return exp.ToString();
        }

        // [NULL TO 200]
        // {100 TO 1000]
        public static FacetRange Parse(string label, string rangeValue)
        {
            var parts = rangeValue.Split(new[] { " TO " }, 2, StringSplitOptions.RemoveEmptyEntries);
            if (parts.Length != 2)
                throw new ArgumentException("Invalid range " + rangeValue + ".");

            var from = parts[0].Trim();
            var to = parts[1].Trim();

            var range = new FacetRange(label);
            range.FromInclusive = IsInclusive(from.First());
            range.ToInclusive = IsInclusive(to.Last());

            from = from.Substring(1);
            to = to.Substring(0, to.Length - 1);

            if (from != "NULL" && from != "*")
            {
                range.FromValue = Convert.ToDouble(from);
            }
            if (to != "NULL" && to != "*")
            {
                range.ToValue = Convert.ToDouble(to);
            }

            return range;
        }

        static bool IsInclusive(char ch)
        {
            switch (ch)
            {
                case '[':
                case ']':
                    return true;
                case '{':
                case '}':
                    return false;
                default:
                    throw new ArgumentException("Could not understand range boundary char " + ch + ".");
            }
        }
    }
}