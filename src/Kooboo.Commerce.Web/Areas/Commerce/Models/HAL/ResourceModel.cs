using Kooboo.Commerce.HAL;
using Kooboo.Commerce.Web.Grid2;
using Kooboo.ComponentModel;
using Kooboo.Web.Mvc.Grid2.Design;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Kooboo.Commerce.Web.Areas.Commerce.Models.HAL
{
    [MetadataFor(typeof(ResourceDescriptor))]
    [Grid(IdProperty = "ResourceName")]
    public class ResourceModel
    {
        [LinkedGridColumn(TargetAction = "Resource", HeaderText = "Resource Name")]
        public string ResourceName { get; set; }

        [BooleanGridColumn(HeaderText = "Is List Resource")]
        public bool IsListResource { get; set; }

        [GridColumn(HeaderText = "Resource URI")]
        public string ResourceUri { get; set; }

        public ResourceModel() { }

        public ResourceModel(ResourceDescriptor resource)
        {
            ResourceName = resource.ResourceName;
            ResourceUri = resource.ResourceUri;
            IsListResource = resource.IsListResource;
        }
    }
}