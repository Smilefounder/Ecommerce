using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace Kooboo.Commerce.CMSIntegration.DataSources.Generic
{
    [DataContract]
    public class SavedFilter
    {
        [DataMember]
        public string Name { get; set; }

        [DataMember]
        public List<SavedFilterParameterValue> ParameterValues { get; set; }

        public SavedFilter()
        {
            ParameterValues = new List<SavedFilterParameterValue>();
        }
    }
}