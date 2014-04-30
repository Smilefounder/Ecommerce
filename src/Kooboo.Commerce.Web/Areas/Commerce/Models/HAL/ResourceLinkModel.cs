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

        public IList<ResourceLinkParameterModel> RequiredParameters { get; set; }

        public IList<ResourceLinkParameterModel> OptionalParameters { get; set; }

        public ResourceLinkModel()
        {
            RequiredParameters = new List<ResourceLinkParameterModel>();
            OptionalParameters = new List<ResourceLinkParameterModel>();
        }

        public ResourceLinkModel(ResourceLink link, ResourceDescriptor destinationResourceDescriptor) : this()
        {
            Id = link.Id;
            Relation = link.Relation;
            DestinationResourceName = link.DestinationResourceName;

            foreach (var linkParam in link.Parameters)
            {
                var param = destinationResourceDescriptor.InputPramameters.FirstOrDefault(p => p.Name == linkParam.Name);
                if (param != null)
                {
                    var paramModel = new ResourceLinkParameterModel
                    {
                        Name = linkParam.Name,
                        Value = linkParam.Value,
                        UseFixedValue = linkParam.UseFixedValue,
                        Required = param.Required,
                        ParameterType = param.ParameterType.Name
                    };

                    if (param.Required)
                    {
                        RequiredParameters.Add(paramModel);
                    }
                    else
                    {
                        OptionalParameters.Add(paramModel);
                    }
                }
            }
        }

        public void UpdateTo(ResourceLink link)
        {
            link.Relation = Relation;
            link.DestinationResourceName = DestinationResourceName;

            var parameters = new List<ResourceLinkParameter>();
            parameters.AddRange(ToResourceLinkParameters(RequiredParameters));
            parameters.AddRange(ToResourceLinkParameters(OptionalParameters));

            link.Parameters = parameters;
        }

        private IEnumerable<ResourceLinkParameter> ToResourceLinkParameters(IEnumerable<ResourceLinkParameterModel> models)
        {
            foreach (var model in models)
            {
                yield return new ResourceLinkParameter
                {
                    Name = model.Name,
                    Value = model.Value,
                    UseFixedValue = model.UseFixedValue
                };
            }
        }
    }
}