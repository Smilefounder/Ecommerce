using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace Kooboo.Commerce.CMSIntegration.DataSources
{
    [DataContract]
    public class DataSourceFilter
    {
        [DataMember]
        public string Name { get; set; }

        [DataMember]
        public List<DataSourceFilterParameterValue> ParameterValues { get; set; }

        public DataSourceFilter()
        {
            ParameterValues = new List<DataSourceFilterParameterValue>();
        }
    }
}