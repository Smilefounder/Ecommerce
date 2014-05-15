using Kooboo.Commerce.API.HAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Kooboo.Commerce.CMSIntegration.DataSources.Models
{
    public class ResourceModel
    {
        public string Name { get; set; }

        public IList<ResourceParameterModel> RequiredParameters { get; set; }

        public IList<ResourceParameterModel> OptionalParameters { get; set; }

        public ResourceModel()
        {
            RequiredParameters = new List<ResourceParameterModel>();
            OptionalParameters = new List<ResourceParameterModel>();
        }

        public ResourceModel(ResourceDescriptor descriptor)
        {
            Name = descriptor.ResourceName.FullName;
            RequiredParameters = descriptor.InputPramameters
                                           .Where(p => p.Required)
                                           .Select(p => new ResourceParameterModel(p))
                                           .ToList();
            OptionalParameters = descriptor.InputPramameters
                                           .Where(p => !p.Required)
                                           .Select(p => new ResourceParameterModel(p))
                                           .ToList();
        }
    }

    public class ResourceParameterModel
    {
        public string Name { get; set; }

        public bool Required { get; set; }

        public ResourceParameterModel() { }

        public ResourceParameterModel(HalParameter param)
        {
            Name = param.Name;
            Required = param.Required;
        }
    }
}