using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace Kooboo.Commerce.Data
{
    public interface IRepository<T> where T : class
    {
        ICommerceDatabase Database { get; }

        T Get(params object[] id);

        IQueryable<T> Query();

        bool Insert(T entity);

        void Update(T entity);

        bool Update(T entity, object values);

        bool Delete(T entity);

        bool InsertBatch(IEnumerable<T> entities);

        bool UpdateBatch(Expression<Func<T, bool>> predicate, Expression<Func<T, T>> setter);

        bool DeleteBatch(Expression<Func<T, bool>> predicate);
    }

    public static class IRepositoryExtensions
    {
        public static T Get<T>(this IRepository<T> repository, Expression<Func<T, bool>> predicate) where T : class
        {
            return repository.Query().Where(predicate).FirstOrDefault();
        }

        public static IQueryable<T> Query<T>(this IRepository<T> repository, Expression<Func<T, bool>> predicate) where T : class
        {
            return repository.Query().Where(predicate);
        }

        public static bool Save<T>(this IRepository<T> repository, Expression<Func<T, bool>> predicate, T obj) where T : class
        {
            if (repository.Query(predicate).Any())
            {
                return repository.Update(obj, obj);
            }
            else
            {
                return repository.Insert(obj);
            }
        }

        public static bool SaveAll<T>(this IRepository<T> repository, ICommerceDatabase db, IEnumerable<T> original, IEnumerable<T> current, Func<T, T, bool> exists
            , bool deleteIfNoFound = true, bool notUpdateIfFound = false)
            where T : class
        {
            if (deleteIfNoFound)
            {
                if (notUpdateIfFound)
                {
                    return SaveAll(repository, db, original, current, exists,
                        (repo, o) => { repo.Insert(o); },
                        null,
                        (repo, o) => { repo.Delete(o); });
                }
                else
                {
                    return SaveAll(repository, db, original, current, exists,
                        (repo, o) => { repo.Insert(o); },
                        (repo, o, c) => { repo.Update(c, c); },
                        (repo, o) => { repo.Delete(o); });
                }
            }
            else
            {
                if (notUpdateIfFound)
                {
                    return SaveAll(repository, db, original, current, exists,
                        (repo, o) => { repo.Insert(o); },
                        null,
                        null);
                }
                else
                {
                    return SaveAll(repository, db, original, current, exists,
                        (repo, o) => { repo.Insert(o); },
                        (repo, o, c) => { repo.Update(c, c); },
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
    }
}
