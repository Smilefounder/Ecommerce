using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Linq.Expressions;
using Kooboo.Web.Mvc.Paging;

namespace Kooboo.Commerce.Data
{
    public interface IRepository<T> where T : class
    {
        ICommerceDatabase Database { get; }

        IQueryable<T> Query(Expression<Func<T, bool>> predicate, Func<IQueryable<T>, IQueryable<T>> orderby, int pageIndex, int pageSize, out int totalRecords);

        T Get(object id);
        
        T Get(Expression<Func<T, bool>> predicate);
        bool Insert(T obj);
        bool Update(T obj, Func<T, object[]> getKeys);
        bool Delete(T obj);

        bool InsertBatch(IEnumerable<T> objs);
        bool UpdateBatch(Expression<Func<T, bool>> predicate, Expression<Func<T, T>> setter);
        bool DeleteBatch(Expression<Func<T, bool>> predicate);
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

        public static bool Save<T>(this IRepository<T> repository, Expression<Func<T, bool>> predicate, T obj, Func<T, object[]> getKeys) where T : class
        {
            if (repository.Query(predicate).Any())
            {
                return repository.Update(obj, getKeys);
            }
            else
            {
                return repository.Insert(obj);
            }
        }

        public static bool SaveAll<T>(this IRepository<T> repository, ICommerceDatabase db, IEnumerable<T> original, IEnumerable<T> current, Func<T, object[]> getKeys, Func<T, T, bool> exists
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
                        (repo, o, c) => { repo.Update(c, getKeys); },
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
                        (repo, o, c) => { repo.Update(c, getKeys); },
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

        public static bool SaveAll<T>(this IRepository<T> repository, ICommerceDatabase db, IEnumerable<T> newItems, IEnumerable<T> modifiedItems, IEnumerable<T> deletedItems, Func<T, object[]> getKeys)
            where T : class
        {
            try
            {
                //db.BeginTransaction();
                newItems.ForEach((o, i) => repository.Insert(o));
                modifiedItems.ForEach((o, i) => repository.Update(o, getKeys));
                deletedItems.ForEach((o, i) => repository.Delete(o));
                //db.Commit();
                return true;
            }
            catch (Exception ex)
            {
                //db.Rollback();
                throw ex;
            }
        }
    }

    public static class PageLinqExtensions
    {
        public static PagedList<T> ToPagedList<T>(this IQueryable<T> allItems, int pageIndex, int pageSize)
        {
            if (pageIndex < 1)
                pageIndex = 1;
            var itemIndex = (pageIndex - 1) * pageSize;
            var pageOfItems = allItems.Skip(itemIndex).Take(pageSize);
            var totalItemCount = allItems.Count();
            return new PagedList<T>(pageOfItems.ToArray(), pageIndex, pageSize, totalItemCount);
        }
        public static PagedList<T> ToPagedList<U, T>(this IQueryable<U> allItems, Func<U, T> converter, int pageIndex, int pageSize)
        {
            if (pageIndex < 1)
                pageIndex = 1;
            var itemIndex = (pageIndex - 1) * pageSize;
            var pageOfItems = allItems.Skip(itemIndex).Take(pageSize);
            var totalItemCount = allItems.Count();
            var data = pageOfItems.ToArray().Select(o => converter(o));
            return new PagedList<T>(data, pageIndex, pageSize, totalItemCount);
        }
    }

    /// <summary>
    /// add this class to allow the json data possibble to be deserialized to page list objects.
    /// </summary>
    public class PageListWrapper<T>
    {
        
        public PageListWrapper()
        { 
        }

        public PageListWrapper(IPagedList<T> pagedList)
        {
            FromPagedList(pagedList);
        }

        public void FromPagedList(IPagedList<T> pagedList)
        {
            this.Data = pagedList;
            this.CurrentPageIndex = pagedList.CurrentPageIndex;
            this.PageSize = pagedList.PageSize;
            this.TotalItemCount = pagedList.TotalItemCount;
        }

        public PagedList<T> ToPagedList()
        {
            return new PagedList<T>(Data, CurrentPageIndex, PageSize, TotalItemCount);
        }

        public IEnumerable<T> Data { get; set; }

        public int CurrentPageIndex { get; set; }
        public int PageSize { get; set; }
        public int TotalItemCount { get; set; }
    }
}