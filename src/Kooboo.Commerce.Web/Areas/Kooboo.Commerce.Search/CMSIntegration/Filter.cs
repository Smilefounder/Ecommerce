using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace Kooboo.Commerce.Search.CMSIntegration
{
    [DataContract]
    [KnownType(typeof(Filter))]
    public class Filter
    {
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
    }
}