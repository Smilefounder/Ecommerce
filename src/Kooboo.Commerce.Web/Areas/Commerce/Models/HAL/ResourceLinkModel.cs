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

        public ResourceModel DestinationResource { get; set; }

        public bool IsEditing { get; set; }
    }
}