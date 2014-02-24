using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.Data
{
    public class CommerceDbProviderInfo
    {
        public string ProviderInvariantName { get; private set; }

        public string ProviderManifestToken { get; private set; }

        public CommerceDbProviderInfo(string providerInvariantName, string providerManifestToken)
        {
            Require.NotNullOrEmpty(providerInvariantName, "providerInvariantName");
            Require.NotNullOrEmpty(providerManifestToken, "providerManifestToken");

            ProviderInvariantName = providerInvariantName;
            ProviderManifestToken = providerManifestToken;
        }

        public override bool Equals(object obj)
        {
            var other = obj as CommerceDbProviderInfo;
            return other != null
                && other.ProviderInvariantName == ProviderInvariantName
                && other.ProviderManifestToken == ProviderManifestToken;
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return (ProviderInvariantName.GetHashCode() * 397) ^ ProviderManifestToken.GetHashCode();
            }
        }

        public override string ToString()
        {
            return ProviderInvariantName + "." + ProviderManifestToken;
        }
    }
}
