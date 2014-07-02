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
    public interface ICommerceDatabase : IDisposable
    {
        CommerceInstanceSettings InstanceSettings { get; }

        IRepository<T> GetRepository<T>() where T : class;

        ICommerceDbTransaction Transaction { get; }

        ICommerceDbTransaction BeginTransaction();

        ICommerceDbTransaction BeginTransaction(IsolationLevel isolationLevel);

        void SaveChanges();
    }
}
