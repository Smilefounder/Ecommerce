using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace Kooboo.Commerce.CMSIntegration.DataSources.Generic
{
    [DataContract]
    public class Filter
    {
        [DataMember]
        public string Name { get; set; }

        [DataMember]
        public List<FilterParameterValue> ParameterValues { get; set; }

        public Filter()
        {
            ParameterValues = new List<FilterParameterValue>();
        }
    }
}