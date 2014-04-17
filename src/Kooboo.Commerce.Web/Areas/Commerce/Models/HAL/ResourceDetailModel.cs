using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Kooboo.Commerce.Web.Areas.Commerce.Models.HAL
{
    public class ResourceDetailModel
    {
        public string ResourceName { get; set; }

        public string ResourceUri { get; set; }

        public bool IsListResource { get; set; }

        public ResourceModel ItemResource { get; set; }

        public IList<ResourceModel> LinkableResources { get; set; }

        public IList<ResourceLinkModel> Links { get; set; }

        public ResourceDetailModel()
        {
            LinkableResources = new List<ResourceModel>();
            Links = new List<ResourceLinkModel>();
        }
    }
}