using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.API.HAL
{
    public class ResourceLink
    {
        public string Id { get; set; }

        public string SourceResourceName { get; set; }

        public string DestinationResourceName { get; set; }

        public string Relation { get; set; }

        /// <summary>
        /// Name of the environment in which this link is available for the source resource.
        /// </summary>
        public string EnvironmentName { get; set; }

        /// <summary>
        /// The parameter mapping from the source resource to the destination resource.
        /// </summary>
        public IDictionary<string, string> ParameterMapping { get; set; }

        public ResourceLink()
        {
            Id = Guid.NewGuid().ToString();
            ParameterMapping = new Dictionary<string, string>();
        }

        public ResourceLink Clone()
        {
            var link = (ResourceLink)MemberwiseClone();
            if (ParameterMapping != null && ParameterMapping.Count > 0)
            {
                link.ParameterMapping = new Dictionary<string, string>(ParameterMapping);
            }
            else
            {
                link.ParameterMapping = new Dictionary<string, string>();
            }

            return link;
        }
    }
}
