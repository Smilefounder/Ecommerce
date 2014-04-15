using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.Hal
{
    public class ResourceLink
    {
        public string Id { get; set; }

        public string SourceResourceName { get; set; }

        public string DestinationResourceName { get; set; }

        public string Rel { get; set; }

        public IDictionary<string, string> ParameterMappings { get; set; }
    }
}
