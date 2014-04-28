using Kooboo.Commerce.API.HAL;
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
        [LinkedGridColumn(TargetAction = "Detail", HeaderText = "Resource Name")]
        public string ResourceName { get; set; }

        [BooleanGridColumn(HeaderText = "Is List Resource")]
        public bool IsListResource { get; set; }

        [GridColumn(HeaderText = "Resource URI")]
        public string ResourceUri { get; set; }

        [GridColumn(HeaderText = "Implicit Link Provider")]
        public ImplicitLinkProviderModel ImplicitLinkProvider { get; set; }

        public IList<HalParameterModel> InputParameters { get; set; }

        public IList<HalParameterModel> OutputParameters { get; set; }

        public ResourceModel()
        {
            InputParameters = new List<HalParameterModel>();
            OutputParameters = new List<HalParameterModel>();
        }

        public ResourceModel(ResourceDescriptor resource)
            : this()
        {
            ResourceName = resource.ResourceName;
            ResourceUri = resource.ResourceUri;
            IsListResource = resource.IsListResource;
            ImplicitLinkProvider = resource.ImplicitLinkProvider == null ? null : new ImplicitLinkProviderModel(resource.ImplicitLinkProvider);

            if (resource.InputPramameters != null && resource.InputPramameters.Length > 0)
            {
                InputParameters = resource.InputPramameters.Select(p => new HalParameterModel(p)).ToList();
            }

            if (resource.OutputParameters != null && resource.OutputParameters.Length > 0)
            {
                OutputParameters = resource.OutputParameters.Select(p => new HalParameterModel(p)).ToList();
            }
        }
    }
}