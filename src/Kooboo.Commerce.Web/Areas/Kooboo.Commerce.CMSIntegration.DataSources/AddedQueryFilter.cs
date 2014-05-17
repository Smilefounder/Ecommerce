using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace Kooboo.Commerce.CMSIntegration.DataSources
{
    [DataContract]
    public class AddedQueryFilter
    {
        [DataMember]
        public string Name { get; set; }

        [DataMember]
        public List<QueryFilterParameterValue> ParameterValues { get; set; }

        public AddedQueryFilter()
        {
            ParameterValues = new List<QueryFilterParameterValue>();
        }
    }
}