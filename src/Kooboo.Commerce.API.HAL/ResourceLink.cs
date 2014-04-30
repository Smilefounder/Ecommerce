using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.API.HAL
{
    public class ResourceLinkParameter
    {
        public string Name { get; set; }

        /// <summary>
        /// The parameter value. It'll be the source parameter name if IsFixedValue is set to false.
        /// </summary>
        public string Value { get; set; }

        /// <summary>
        /// Indicates if the parameter value is a fixed value. 
        /// If not fixed, the parameter value will store the name of the parameter from which the parameter value is dynamically retrieved at runtime.
        /// </summary>
        public bool UseFixedValue { get; set; }

        public ResourceLinkParameter Clone()
        {
            return (ResourceLinkParameter)MemberwiseClone();
        }
    }

    public class ResourceLink
    {
        public string Id { get; set; }

        public string SourceResourceName { get; set; }

        public string DestinationResourceName { get; set; }

        public string Relation { get; set; }

        /// <summary>
        /// The parameters used in this resource link. It's a subset of the input parameters of the destination resource.
        /// </summary>
        public IList<ResourceLinkParameter> Parameters { get; set; }

        public ResourceLink()
        {
            Id = Guid.NewGuid().ToString();
            Parameters = new List<ResourceLinkParameter>();
        }

        public ResourceLink Clone()
        {
            var link = (ResourceLink)MemberwiseClone();
            link.Parameters = Parameters.Select(v => v.Clone()).ToList();
            return link;
        }

        /// <summary>
        /// Copy link information to the other link without the Id.
        /// </summary>
        internal void CopyTo(ResourceLink other)
        {
            other.SourceResourceName = SourceResourceName;
            other.Relation = Relation;
            other.DestinationResourceName = DestinationResourceName;
            other.Parameters = Parameters.Select(v => v.Clone()).ToList();
        }
    }
}
