using Kooboo.Commerce.Api;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace Kooboo.Commerce.CMSIntegration.DataSources.Generic
{
    [DataContract]
    public class GenericCommerceDataSourceSettings
    {
        [DataMember]
        public List<SavedFilter> Filters { get; set; }

        [DataMember]
        public List<string> Includes { get; set; }

        [DataMember]
        public string Top { get; set; }

        [DataMember]
        public string SortField { get; set; }

        [DataMember]
        public SortDirection SortDirection { get; set; }

        [DataMember]
        public bool EnablePaging { get; set; }

        [DataMember]
        public string PageSize { get; set; }

        [DataMember]
        public string PageNumber { get; set; }

        [DataMember]
        public TakeOperation TakeOperation { get; set; }

        public GenericCommerceDataSourceSettings()
        {
            Filters = new List<SavedFilter>();
            Includes = new List<string>();
        }
    }
}