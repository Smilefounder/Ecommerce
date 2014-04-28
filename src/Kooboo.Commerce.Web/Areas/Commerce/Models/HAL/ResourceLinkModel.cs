using Kooboo.Commerce.API.HAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Kooboo.Commerce.Web.Areas.Commerce.Models.HAL
{
    public class ResourceLinkModel
    {
        public string Id { get; set; }

        public string Relation { get; set; }

        public string DestinationResourceName { get; set; }

        public IList<HalParameterValue> DestinationResourceParameterValues { get; set; }

        public ResourceLinkModel()
        {
            DestinationResourceParameterValues = new List<HalParameterValue>();
        }

        public ResourceLinkModel(ResourceLink link)
        {
            Id = link.Id;
            Relation = link.Relation;
            DestinationResourceName = link.DestinationResourceName;
            DestinationResourceParameterValues = link.DestinationResourceParameterValues.ToList();
        }

        public void UpdateTo(ResourceLink link)
        {
            link.Relation = Relation;
            link.DestinationResourceName = DestinationResourceName;
            link.DestinationResourceParameterValues = DestinationResourceParameterValues;
        }
    }
}