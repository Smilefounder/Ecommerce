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
        private IEnumerable<IRepositoryEventHandler<T>> _events;

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
        public CommerceRepository(ICommerceDatabase database, IEnumerable<IRepositoryEventHandler<T>> events)
        {
            Require.NotNull(database, "database");
            Require.That(database is CommerceDatabase, "Requires type " + typeof(CommerceDatabase) + ".");

            _database = (CommerceDatabase)database;
            _events = events;
            _events = new List<IRepositoryEventHandler<T>>();
        }

        public IQueryable<T> Query(Expression<Func<T, bool>> predicate, Func<IQueryable<T>, IQueryable<T>> orderby, int pageIndex, int pageSize, out int totalRecords)
        {
            totalRecords = 0;
            IQueryable<T> query = DbContext.Set<T>();
            if (predicate != null)
                query = query.Where(predicate);
            if (orderby != null)
                query = orderby(query);
            if (pageIndex >= 0 && pageSize > 0)
            {
                totalRecords = query.Count();
                query = query.Skip(pageIndex * pageSize).Take(pageSize);
            }

            return query;
        }

        public virtual T Get(Expression<Func<T, bool>> predicate)
        {
            IQueryable<T> query = DbContext.Set<T>();
            if (predicate != null)
                query = query.Where(predicate);
            return query.FirstOrDefault();
        }

        public virtual bool Insert(T obj, EntityEventHandler<T> insertingHandler = null, EntityEventHandler<T> insertedHandler = null)
        {
            obj = this.HandleEvents(_events, es => es.InsertingHandler, DataOperation.Insert, null, obj);
            obj = this.HandleEvent(insertingHandler, DataOperation.Insert, null, obj);

            var tbl = DbContext.Set<T>();
            obj = tbl.Add(obj);

            int ret = DbContext.SaveChanges();

            if (ret > 0)
            {
                this.HandleEvent(insertedHandler, DataOperation.Insert, null, obj);
                this.HandleEvents(_events, es => es.InsertedHandler, DataOperation.Insert, null, obj);
            }

            return ret > 0;
        }

        public virtual bool Update(T obj, Func<T, object[]> getKeys, EntityEventHandler<T> updatingHandler = null, EntityEventHandler<T> updatedHandler = null)
        {
            obj = this.HandleEvents(_events, es => es.UpdatingHandler, DataOperation.Update, obj, obj);
            obj = this.HandleEvent(updatingHandler, DataOperation.Update, obj, obj);

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

            if (ret > 0)
            {
                this.HandleEvent(updatedHandler, DataOperation.Update, obj, obj);
                this.HandleEvents(_events, es => es.UpdatedHandler, DataOperation.Update, obj, obj);
            }

            return ret > 0;
        }

        public virtual bool Delete(T obj, EntityEventHandler<T> deletingHandler = null, EntityEventHandler<T> deletedHandler = null)
        {
            obj = this.HandleEvents(_events, es => es.DeletingHandler, DataOperation.Delete, obj, null);
            obj = this.HandleEvent(deletingHandler, DataOperation.Delete, obj, null);

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

            if (ret > 0)
            {
                this.HandleEvent(deletedHandler, DataOperation.Delete, obj, null);
                this.HandleEvents(_events, es => es.DeletedHandler, DataOperation.Delete, obj, null);
            }

            return ret > 0;
        }

        public virtual bool InsertBatch(IEnumerable<T> objs, BatchEntityEventHandler<T> batchInsertingHandler = null, BatchEntityEventHandler<T> batchInsertedHandler = null)
        {
            objs = this.HandleBatchEvents(_events, es => es.BatchInsertingHandler, DataOperation.Insert, objs);
            objs = this.HandleBatchEvent(batchInsertingHandler, DataOperation.Insert, objs);

            var tbl = DbContext.Set<T>();
            foreach (var obj in objs)
            {
                tbl.Add(obj);
            }

            int totals = DbContext.SaveChanges();

            if (totals > 0)
            {
                this.HandleBatchEvent(batchInsertedHandler, DataOperation.Insert, objs);
                this.HandleBatchEvents(_events, es => es.BatchInsertedHandler, DataOperation.Insert, objs);
            }

            return totals > 0;
        }

        public virtual bool UpdateBatch(Expression<Func<T, bool>> predicate, Expression<Func<T, T>> setter, BatchEntityEventHandler<T> batchUpdatingHandler = null, BatchEntityEventHandler<T> batchUpdatedHandler = null)
        {
            IQueryable<T> query = DbContext.Set<T>();
            if (predicate != null)
                query = query.Where(predicate);
            IEnumerable<T> objs = query.ToArray();
            objs = this.HandleBatchEvents(_events, es => es.BatchUpdatingHandler, DataOperation.Update, objs);
            objs = this.HandleBatchEvent(batchUpdatingHandler, DataOperation.Update, objs);

            var func = setter.Compile();
            foreach (var obj in objs)
            {
                func(obj);
            }

            int totals = DbContext.SaveChanges();

            if (totals > 0)
            {
                this.HandleBatchEvent(batchUpdatedHandler, DataOperation.Update, objs);
                this.HandleBatchEvents(_events, es => es.BatchUpdatedHandler, DataOperation.Update, objs);
            }

            return totals > 0;
        }

        public virtual bool DeleteBatch(Expression<Func<T, bool>> predicate, BatchEntityEventHandler<T> batchDeletingHandler = null, BatchEntityEventHandler<T> batchDeletedHandler = null)
        {
            var tbl = DbContext.Set<T>();
            IQueryable<T> query = tbl;
            if (predicate != null)
                query = query.Where(predicate);
            IEnumerable<T> objs = query.ToArray();

            objs = this.HandleBatchEvents(_events, es => es.BatchDeletingHandler, DataOperation.Delete, objs);
            objs = this.HandleBatchEvent(batchDeletingHandler, DataOperation.Delete, objs);

            foreach (var entity in objs)
                tbl.Remove(entity);

            int ret = DbContext.SaveChanges();

            if (ret > 0)
            {
                this.HandleBatchEvent(batchDeletedHandler, DataOperation.Delete, objs);
                this.HandleBatchEvents(_events, es => es.BatchDeletedHandler, DataOperation.Delete, objs);
            }

            return ret > 0;
        }
    }

}