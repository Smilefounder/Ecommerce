using Kooboo.CMS.Common.Runtime.Dependency;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.Entity.Core.Common;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.Data.SqlServer
{
    [Dependency(typeof(ICommerceDbProvider), ComponentLifeStyle.Singleton, Key = "Kooboo.Commerce.Data.SqlServerCommerceDbProvider")]
    public class SqlServerCommerceDbProvider : ICommerceDbProvider
    {
        public string InvariantName
        {
            get
            {
                return "System.Data.SqlClient";
            }
        }

        public string ManifestToken
        {
            get
            {
                return "2012";
            }
        }

        public string DisplayName
        {
            get
            {
                return "Microsoft SQL Server 2012";
            }
        }

        public ICommerceDatabaseOperations DatabaseOperations
        {
            get
            {
                return new SqlServerCommerceDatabaseOperations();
            }
        }

        public IConnectionStringBuilder ConnectionStringBuilder
        {
            get
            {
                return new SqlServerConnectionStringBuilder();
            }
        }
    }

    public class SqlServerConnectionStringBuilder : IConnectionStringBuilder
    {
        public IEnumerable<string> ParameterNames
        {
            get
            {
                yield return "Server";
                yield return "Database";
                yield return "User ID";
                yield return "Password";
            }

        }

        public string BuildConnectionString(IDictionary<string, string> parameters)
        {
            var connectionString = new StringBuilder();

            foreach (var name in ParameterNames)
            {
                if (!parameters.ContainsKey(name))
                    throw new InvalidOperationException("Missing SQL Server connection string parameter: " + name + ".");

                connectionString.AppendFormat("{0}={1};", name, parameters[name]);
            }

            connectionString.Append("MultipleActiveResultSets=true");

            return connectionString.ToString();
        }
    }
}
