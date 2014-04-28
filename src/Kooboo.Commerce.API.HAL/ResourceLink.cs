using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.API.HAL
{
    public class HalParameterValue
    {
        public string ParameterName { get; set; }

        /// <summary>
        /// The parameter value. It'll be the source parameter name if IsFixedValue is set to false.
        /// </summary>
        public string ParameterValue { get; set; }

        /// <summary>
        /// Indicates if the parameter value is a fixed value. 
        /// If not fixed, the parameter value will store the name of the parameter from which the parameter value is dynamically retrieved at runtime.
        /// </summary>
        public bool IsFixedValue { get; set; }

        public HalParameterValue Clone()
        {
            return (HalParameterValue)MemberwiseClone();
        }
    }

    public class ResourceLink
    {
        public string Id { get; set; }

        public string SourceResourceName { get; set; }

        public string DestinationResourceName { get; set; }

        public string Relation { get; set; }

        public IList<HalParameterValue> DestinationResourceParameterValues { get; set; }

        public ResourceLink()
        {
            Id = Guid.NewGuid().ToString();
            DestinationResourceParameterValues = new List<HalParameterValue>();
        }

        public ResourceLink Clone()
        {
            var link = (ResourceLink)MemberwiseClone();
            link.DestinationResourceParameterValues = DestinationResourceParameterValues.Select(v => v.Clone()).ToList();
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
            other.DestinationResourceParameterValues = DestinationResourceParameterValues.Select(v => v.Clone()).ToList();
        }
    }
}
