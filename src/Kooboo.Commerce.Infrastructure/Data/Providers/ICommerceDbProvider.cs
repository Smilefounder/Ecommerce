using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.Data.Providers
{
    // TODO: Refactor this
    public interface ICommerceDbProvider
    {
        string InvariantName { get; }

        string ManifestToken { get; }

        string DisplayName { get; }

        ICommerceDatabaseOperations DatabaseOperations { get; }

        IConnectionStringBuilder ConnectionStringBuilder { get; }

        void Initialize();
    }

    public interface IConnectionStringBuilder
    {
        IEnumerable<string> ParameterNames { get; }

        string BuildConnectionString(IDictionary<string, string> parameters);
    }

    static class CommerceDbProviderExtensions
    {
        public static string GetConnectionString(this ICommerceDbProvider dbProvider, CommerceInstanceSettings metadata)
        {
            if (String.IsNullOrEmpty(metadata.ConnectionString))
            {
                return dbProvider.ConnectionStringBuilder.BuildConnectionString(metadata.ConnectionStringParameters);
            }

            return metadata.ConnectionString;
        }
    }
}
