using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.Data
{
    public class CommerceInstanceSettings
    {
        public string Name { get; set; }

        public string DisplayName { get; set; }

        public string DbSchema { get; set; }

        public string DbProviderInvariantName { get; set; }

        public string DbProviderManifestToken { get; set; }

        public DateTime CreatedAtUtc { get; set; }

        public string ConnectionString { get; set; }

        public IDictionary<string, string> ConnectionStringParameters { get; set; }

        public CommerceInstanceSettings()
        {
            CreatedAtUtc = DateTime.UtcNow;
            ConnectionStringParameters = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
        }

        public CommerceInstanceSettings Clone()
        {
            return (CommerceInstanceSettings)base.MemberwiseClone();
        }
    }
}
