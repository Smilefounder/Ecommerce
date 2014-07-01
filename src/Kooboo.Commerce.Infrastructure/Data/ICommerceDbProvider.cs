using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.Data
{
    public interface ICommerceDbProvider
    {
        string InvariantName { get; }

        string ManifestToken { get; }

        string DisplayName { get; }

        ICommerceDatabaseOperations DatabaseOperations { get; }

        IConnectionStringBuilder ConnectionStringBuilder { get; }
    }

    public interface IConnectionStringBuilder
    {
        IEnumerable<string> ParameterNames { get; }

        string BuildConnectionString(IDictionary<string, string> parameters);
    }

    static class CommerceDbProviderExtensions
    {
        public static string GetConnectionString(this ICommerceDbProvider dbProvider, CommerceInstanceMetadata metadata)
        {
            if (String.IsNullOrEmpty(metadata.ConnectionString))
            {
                return dbProvider.ConnectionStringBuilder.BuildConnectionString(metadata.ConnectionStringParameters);
            }

            return metadata.ConnectionString;
        }
    }
}
