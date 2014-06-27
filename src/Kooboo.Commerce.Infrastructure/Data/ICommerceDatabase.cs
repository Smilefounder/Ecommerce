using Kooboo.CMS.Common.Runtime.Dependency;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.Data
{
    /// <summary>
    /// Represents an abstraction of a database for commerce instance data.
    /// It can be a physical database, or just a set of tables with a specific schema in a database.
    /// </summary>
    /// <remarks>
    /// If transaction is not explicitly used, then each call to a repository method is performed within an implicit transaction.
    /// This has benifits that each entity insert can return entity identity immediately.
    /// This is different from O/R mapping frameworks unit of work implementation.
    /// So we don't need methods like Commit or SaveChanges here.
    /// </remarks>
    public interface ICommerceDatabase : IDisposable
    {
        InstanceMetadata InstanceMetadata { get; }

        IRepository<T> GetRepository<T>() where T : class;

        ICommerceDbTransaction Transaction { get; }

        ICommerceDbTransaction BeginTransaction();

        ICommerceDbTransaction BeginTransaction(IsolationLevel isolationLevel);

        void SaveChanges();
    }
}
