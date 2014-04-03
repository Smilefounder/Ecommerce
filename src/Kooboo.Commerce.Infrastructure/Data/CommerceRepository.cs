using Kooboo.CMS.Common.Runtime.Dependency;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Linq.Expressions;

namespace Kooboo.Commerce.Data
{
    public class CommerceRepository<T> : IRepository<T> where T : class
    {
        private CommerceDatabase _database;

        public ICommerceDatabase Database
        {
            get
            {
                return _database;
            }
        }

        protected DbContext DbContext
        {
            get
            {
                return _database.DbContext;
            }
        }

        // Passing in ICommerceDatabase instead of CommerceDatabase is because,
        // in IoC container the service type is ICommerceDatabase.
        // If we ask for CommerceDatabase here, the IoC container will not be able to provide the CommerceDatabase instance.
        public CommerceRepository(ICommerceDatabase database)
        {
            Require.NotNull(database, "database");
            Require.That(database is CommerceDatabase, "Requires type " + typeof(CommerceDatabase) + ".");

            _database = (CommerceDatabase)database;
        }

        public IQueryable<T> Query()
        {
            return DbContext.Set<T>();
        }

        public virtual T Get(params object[] id)
        {
            return DbContext.Set<T>().Find(id);
        }

        public virtual bool Insert(T obj)
        {
            if (obj == null)
                return false;

            var tbl = DbContext.Set<T>();
            obj = tbl.Add(obj);

            int ret = DbContext.SaveChanges();

            return ret > 0;
        }

        public virtual bool Update(T obj, Func<T, object[]> getKeys)
        {
            if (obj == null)
                return false;
            var entry = DbContext.Entry(obj);

            if (entry.State == EntityState.Detached)
            {
                var tbl = DbContext.Set<T>();
                var keys = getKeys(obj);
                var currentEntry = tbl.Find(keys);
                if (currentEntry != null)
                {
                    var attachedEntry = DbContext.Entry(currentEntry);
                    attachedEntry.CurrentValues.SetValues(obj);
                    attachedEntry.State = EntityState.Modified;
                }
                else
                {
                    tbl.Attach(obj);
                    entry.State = EntityState.Modified;
                }
            }

            int ret = DbContext.SaveChanges();

            return ret > 0;
        }

        public virtual bool Delete(T obj)
        {
            if (obj == null)
                return false;
            var tbl = DbContext.Set<T>();
            if (!tbl.Local.Contains(obj))
            {
                DbContext.Entry(obj).State = EntityState.Deleted;
            }
            else
            {
                DbContext.Set<T>().Remove(obj);
            }

            int ret = DbContext.SaveChanges();

            return ret > 0;
        }

        public virtual bool InsertBatch(IEnumerable<T> objs)
        {
            if (objs == null || objs.Count() <= 0)
                return false;
            var tbl = DbContext.Set<T>();
            foreach (var obj in objs)
            {
                tbl.Add(obj);
            }

            int totals = DbContext.SaveChanges();

            return totals > 0;
        }

        public virtual bool UpdateBatch(Expression<Func<T, bool>> predicate, Expression<Func<T, T>> setter)
        {
            IQueryable<T> query = DbContext.Set<T>();
            if (predicate != null)
                query = query.Where(predicate);
            IEnumerable<T> objs = query.ToArray();

            if (objs == null || objs.Count() <= 0)
                return false;
            var func = setter.Compile();
            foreach (var obj in objs)
            {
                func(obj);
            }

            int totals = DbContext.SaveChanges();

            return totals > 0;
        }

        public virtual bool DeleteBatch(Expression<Func<T, bool>> predicate)
        {
            var tbl = DbContext.Set<T>();
            IQueryable<T> query = tbl;
            if (predicate != null)
                query = query.Where(predicate);
            IEnumerable<T> objs = query.ToArray();
            if (objs == null || objs.Count() <= 0)
                return false;

            foreach (var entity in objs)
                tbl.Remove(entity);

            int ret = DbContext.SaveChanges();

            return ret > 0;
        }
    }

}