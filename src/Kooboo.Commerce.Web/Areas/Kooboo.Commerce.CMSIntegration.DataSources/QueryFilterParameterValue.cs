using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace Kooboo.Commerce.CMSIntegration.DataSources
{
    [DataContract]
    public class QueryFilterParameterValue
    {
        [DataMember]
        public string ParameterName { get; set; }

        [DataMember]
        public string ParameterValue { get; set; }
    }
}