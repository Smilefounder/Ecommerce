using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Linq.Expressions;

namespace Kooboo.Commerce.Data
{
    public interface IRepository<T> where T : class
    {
        ICommerceDatabase Database { get; }

        IQueryable<T> Query(Expression<Func<T, bool>> predicate, Func<IQueryable<T>, IQueryable<T>> orderby, int pageIndex, int pageSize, out int totalRecords);
        T Get(Expression<Func<T, bool>> predicate);
        bool Insert(T obj, EntityEventHandler<T> insertingHandler = null, EntityEventHandler<T> insertedHandler = null);
        bool Update(T obj, Func<T, object[]> getKeys, EntityEventHandler<T> updatingHandler = null, EntityEventHandler<T> updatedHandler = null);
        bool Delete(T obj, EntityEventHandler<T> deletingHandler = null, EntityEventHandler<T> deletedHandler = null);

        bool InsertBatch(IEnumerable<T> objs, BatchEntityEventHandler<T> batchInsertingHandler = null, BatchEntityEventHandler<T> batchInsertedHandler = null);
        bool UpdateBatch(Expression<Func<T, bool>> predicate, Expression<Func<T, T>> setter, BatchEntityEventHandler<T> batchUpdatingHandler = null, BatchEntityEventHandler<T> batchUpdatedHandler = null);
        bool DeleteBatch(Expression<Func<T, bool>> predicate, BatchEntityEventHandler<T> batchDeletingHandler = null, BatchEntityEventHandler<T> batchDeletedHandler = null);
    }

    public static class IRepositoryExtensions
    {
        public static IQueryable<T> Query<T>(this IRepository<T> repository, Expression<Func<T, bool>> predicate = null, Func<IQueryable<T>, IQueryable<T>> orderby = null) where T : class
        {
            int totalRecords = 0;
            return repository.Query(predicate, orderby, 0, 0, out totalRecords);
        }

        public static IQueryable<T> Query<T>(this IRepository<T> repository, int pageIndex, int pageSize, out int totalRecords) where T : class
        {
            return repository.Query(null, null, 0, 0, out totalRecords);
        }

        public static IQueryable<T> Query<T>(this IRepository<T> repository, string predicate, object[] values, Func<IQueryable<T>, IQueryable<T>> orderby = null) where T : class
        {
            int totalRecords = 0;
            return Query(repository, predicate, values, orderby, 0, 0, out totalRecords);
        }

        public static IQueryable<T> Query<T>(this IRepository<T> repository, string predicate, object[] values, Func<IQueryable<T>, IQueryable<T>> orderby, int pageIndex, int pageSize, out int totalRecords) where T : class
        {
            Expression<Func<T, bool>> exp = null;
            if (!string.IsNullOrWhiteSpace(predicate))
                exp = DynamicExpression.ParseLambda<T, bool>(predicate, values);
            totalRecords = 0;
            return repository.Query(exp, orderby, pageIndex, pageSize, out totalRecords);
        }

        public static bool Save<T>(this IRepository<T> repository, Expression<Func<T, bool>> predicate, T obj, Func<T, object[]> getKeys, EntityEventHandler<T> insertingHandler = null, EntityEventHandler<T> insertedHandler = null, EntityEventHandler<T> updatingHandler = null, EntityEventHandler<T> updatedHandler = null) where T : class
        {
            if (repository.Query(predicate).Any())
            {
                return repository.Update(obj, getKeys, updatingHandler, updatedHandler);
            }
            else
            {
                return repository.Insert(obj, insertingHandler, insertedHandler);
            }
        }

        public static bool SaveAll<T>(this IRepository<T> repository, ICommerceDatabase db, IEnumerable<T> original, IEnumerable<T> current, Func<T, object[]> getKeys, Func<T, T, bool> exists
            , bool deleteIfNoFound = true, bool notUpdateIfFound = false
            , EntityEventHandler<T> insertingHandler = null, EntityEventHandler<T> insertedHandler = null
            , EntityEventHandler<T> updatingHandler = null, EntityEventHandler<T> updatedHandler = null
            , EntityEventHandler<T> deletingHandler = null, EntityEventHandler<T> deletedHandler = null)
            where T : class
        {
            if (deleteIfNoFound)
            {
                if (notUpdateIfFound)
                {
                    return SaveAll(repository, db, original, current, exists,
                        (repo, o) => { repo.Insert(o, insertingHandler, insertedHandler); },
                        null,
                        (repo, o) => { repo.Delete(o, deletingHandler, deletedHandler); });
                }
                else
                {
                    return SaveAll(repository, db, original, current, exists,
                        (repo, o) => { repo.Insert(o, insertingHandler, insertedHandler); },
                        (repo, o, c) => { repo.Update(c, getKeys, updatingHandler, updatedHandler); },
                        (repo, o) => { repo.Delete(o, deletingHandler, deletedHandler); });
                }
            }
            else
            {
                if (notUpdateIfFound)
                {
                    return SaveAll(repository, db, original, current, exists,
                        (repo, o) => { repo.Insert(o, insertingHandler, insertedHandler); },
                        null,
                        null);
                }
                else
                {
                    return SaveAll(repository, db, original, current, exists,
                        (repo, o) => { repo.Insert(o, insertingHandler, insertedHandler); },
                        (repo, o, c) => { repo.Update(c, getKeys, updatingHandler, updatedHandler); },
                        null);
                }
            }
        }

        public static bool SaveAll<T>(this IRepository<T> repository, ICommerceDatabase db, IEnumerable<T> original, IEnumerable<T> current, Func<T, T, bool> exists, Action<IRepository<T>, T> actionOriginalNotFound, Action<IRepository<T>, T, T> actionFound, Action<IRepository<T>, T> actionCurrentNotFound)
            where T : class
        {
            try
            {
                //db.BeginTransaction();
                if (original == null || original.Count() <= 0)
                {
                    if (current == null || current.Count() <= 0)
                        return false;

                    if (actionOriginalNotFound != null)
                    {
                        foreach (T obj in current)
                        {
                            actionOriginalNotFound(repository, obj);
                        }
                    }
                    //db.Commit();
                    return true;
                }
                else
                {
                    if (current == null || current.Count() <= 0)
                    {
                        if (actionCurrentNotFound != null)
                        {
                            foreach (T obj in original)
                            {
                                actionCurrentNotFound(repository, obj);
                            }
                        }
                        //db.Commit();
                        return true;
                    }
                    else
                    {
                        if (actionOriginalNotFound != null)
                        {
                            var insertedObjs = current.Where(o => original.Any(c => exists(c, o)) == false);
                            foreach (var obj in insertedObjs)
                            {
                                actionOriginalNotFound(repository, obj);
                            }
                        }

                        if (actionFound != null)
                        {
                            var updatedObjs = current.Where(o => original.Any(c => exists(c, o)) == true);
                            foreach (var obj in updatedObjs)
                            {
                                actionFound(repository, original.First(c => exists(c, obj)), obj);
                            }
                        }

                        if (actionCurrentNotFound != null)
                        {
                            var deletedObjs = original.Where(o => current.Any(c => exists(c, o)) == false);
                            foreach (var obj in deletedObjs)
                            {
                                actionCurrentNotFound(repository, obj);
                            }
                        }
                        //db.Commit();
                        return true;
                    }
                }
            }
            catch (Exception ex)
            {
                //db.Rollback();
                throw ex;
            }
        }

        public static bool SaveAll<T>(this IRepository<T> repository, ICommerceDatabase db, IEnumerable<T> newItems, IEnumerable<T> modifiedItems, IEnumerable<T> deletedItems, Func<T, object[]> getKeys
            , EntityEventHandler<T> insertingHandler = null, EntityEventHandler<T> insertedHandler = null
            , EntityEventHandler<T> updatingHandler = null, EntityEventHandler<T> updatedHandler = null
            , EntityEventHandler<T> deletingHandler = null, EntityEventHandler<T> deletedHandler = null)
            where T : class
        {
            try
            {
                //db.BeginTransaction();
                newItems.ForEach((o, i) => repository.Insert(o, insertingHandler, insertedHandler));
                modifiedItems.ForEach((o, i) => repository.Update(o, getKeys, updatingHandler, updatedHandler));
                deletedItems.ForEach((o, i) => repository.Delete(o, deletingHandler, deletedHandler));
                //db.Commit();
                return true;
            }
            catch (Exception ex)
            {
                //db.Rollback();
                throw ex;
            }
        }

        #region Handle Events

        public static T HandleEvent<T>(this IRepository<T> repository, EntityEventHandler<T> handler, DataOperation action, T original, T obj)
            where T : class
        {
            if (handler == null)
                return obj == null ? original : obj;
            var args = new EntityEventArgs<T>() { Action = action, OriginalEntity = original, Entity = obj, Repository = repository };
            handler(args);
            return args.Entity;
        }

        public static IEnumerable<T> HandleBatchEvent<T>(this IRepository<T> repository, BatchEntityEventHandler<T> handler, DataOperation action, IEnumerable<T> objs)
            where T : class
        {
            if (handler == null)
                return objs;
            var args = new EntitiesEventArgs<T>() { Entities = objs, Action = action, Count = objs.Count(), Repository = repository };
            handler(args);
            return args.Entities;
        }

        public static bool CanHandle<T>(this IRepository<T> repository, IEnumerable<IRepositoryEventHandler<T>> events)
            where T : class
        {
            if (events == null || events.Count() <= 0)
                return false;
            return events.Any(o => o.CanHandle());
        }

        public static T HandleEvents<T>(this IRepository<T> repository, IEnumerable<IRepositoryEventHandler<T>> events, Func<IRepositoryEventHandler<T>, EntityEventHandler<T>> func, DataOperation action, T original, T obj)
            where T : class
        {
            if (events == null || events.Count() <= 0)
                return obj == null ? original : obj;
            T nobjs = obj;
            foreach (var eve in events)
            {
                var handler = func(eve);
                nobjs = HandleEvent(repository, handler, action, original, nobjs);
            }
            return nobjs;
        }

        public static IEnumerable<T> HandleBatchEvents<T>(this IRepository<T> repository, IEnumerable<IRepositoryEventHandler<T>> events, Func<IRepositoryEventHandler<T>, BatchEntityEventHandler<T>> func, DataOperation action, IEnumerable<T> objs)
            where T : class
        {
            if (events == null || events.Count() <= 0)
                return objs;
            IEnumerable<T> nobjs = objs;
            foreach (var eve in events)
            {
                var handler = func(eve);
                nobjs = HandleBatchEvent(repository, handler, action, nobjs);
            }
            return nobjs;
        }

        #endregion
    }
}