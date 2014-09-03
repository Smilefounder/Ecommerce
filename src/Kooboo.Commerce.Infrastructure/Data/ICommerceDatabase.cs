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

        IRepository Repository(Type entityType);

        IRepository<T> Repository<T>() where T : class;

        ICommerceDbTransaction Transaction { get; }

        ICommerceDbTransaction BeginTransaction();

        ICommerceDbTransaction BeginTransaction(IsolationLevel isolationLevel);

        void SaveChanges();
    }

    public static class CommerceDatabaseExtensions
    {
        public static T Find<T>(this ICommerceDatabase database, params object[] ids)
            where T : class
        {
            return database.Repository<T>().Find(ids);
        }

        public static IQueryable<T> Query<T>(this ICommerceDatabase database)
            where T : class
        {
            return database.Repository<T>().Query();
        }

        public static void Insert<T>(this ICommerceDatabase database, T entity)
            where T : class
        {
            database.Repository<T>().Insert(entity);
        }

        public static void Delete<T>(this ICommerceDatabase database, T entity)
            where T : class
        {
            database.Repository<T>().Delete(entity);
        }
    }
}
