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

        public ResourceLink()
        {
            Id = Guid.NewGuid().ToString();
        }

        public ResourceLink Clone()
        {
            return (ResourceLink)MemberwiseClone();
        }
    }
}
