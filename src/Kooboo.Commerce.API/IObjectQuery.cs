using Kooboo.Commerce.API.Expressions;
using Kooboo.Commerce.API.Products;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace Kooboo.Commerce.API
{
    public class Test
    {
        public static void M()
        {
            ICommerceAPI api = null;

            var payments = api.Payments
                              .Query()
                              .Where(x => x.Amount > 50)
                              .OrderByDescending(x => x.Id)
                              .Take(10)
                              .ToList();


        }
    }

    public interface IObjectQuery<T>
        where T : class
    {
        IObjectQuery<T> Where(Expression<Func<T, bool>> predicate);

        IObjectQuery<T> Skip(int count);

        IObjectQuery<T> Take(int count);

        IObjectQuery<T> OrderBy<TKey>(Expression<Func<T, TKey>> keySelector);

        IObjectQuery<T> OrderByDescending<TKey>(Expression<Func<T, TKey>> keySelector);

        IPagedList<T> Paginate(int pageIndex, int pageSize);

        int Count();

        T FirstOrDefault();

        List<T> ToList();
    }

    public class ObjectQuery<T> : IObjectQuery<T>
        where T : class
    {
        private Expression _predicate;
        private int _take;
        private int _skip;
        private IQueryExecutor _executor;
        private List<SortDefinition> _sortDefinitions = new List<SortDefinition>();

        public ObjectQuery(IQueryExecutor executor)
        {
            _executor = executor;
        }

        private ObjectQuery(IQueryExecutor executor, Expression expression, int skip, int take)
        {
            _executor = executor;
            _predicate = expression;
            _skip = skip;
            _take = take;
        }

        public IObjectQuery<T> Where(Expression<Func<T, bool>> predicate)
        {
            var query = Clone();

            if (_predicate == null)
            {
                query._predicate = predicate;
            }
            else
            {
                query._predicate = Expression.AndAlso(query._predicate, predicate);
            }

            return query;
        }

        public IObjectQuery<T> Skip(int count)
        {
            var query = Clone();
            query._skip = count;
            return query;
        }

        public IObjectQuery<T> Take(int count)
        {
            var query = Clone();
            query._take = count;
            return query;
        }

        public IObjectQuery<T> OrderBy<TKey>(Expression<Func<T, TKey>> keySelector)
        {
            var query = Clone();
            query._sortDefinitions.Add(new SortDefinition
            {
                Property = PropertyPathBuilder.BuildPropertyPath<T, TKey>(keySelector),
                Direction = SortDirection.Asc
            });
            return query;
        }

        public IObjectQuery<T> OrderByDescending<TKey>(Expression<Func<T, TKey>> keySelector)
        {
            var query = Clone();
            query._sortDefinitions.Add(new SortDefinition
            {
                Property = PropertyPathBuilder.BuildPropertyPath<T, TKey>(keySelector),
                Direction = SortDirection.Desc
            });
            return query;
        }

        public IPagedList<T> Paginate(int pageIndex, int pageSize)
        {
            return _executor.ExecutePagedList<T>(this, new PaginationOptions
            {
                PageIndex = pageIndex,
                PageSize = pageSize,
                Predicate = _predicate,
                SortDefinitions = _sortDefinitions.ToList()
            });
        }

        public int Count()
        {
            return (int)_executor.ExecuteScalar<T>(this, new ScalarOptions { ScalarMethod = "Count" });
        }

        public T FirstOrDefault()
        {
            return Take(1).ToList().FirstOrDefault();
        }

        public List<T> ToList()
        {
            return _executor.ExecuteList<T>(this, new ListOptions
            {
                Skip = _skip,
                Take = _take,
                Predicate = _predicate,
                SortDefinitions = _sortDefinitions.ToList()
            });
        }

        private ObjectQuery<T> Clone()
        {
            var query = new ObjectQuery<T>(_executor, _predicate, _skip, _take);
            query._sortDefinitions = _sortDefinitions.Select(x => x.Clone()).ToList();
            return query;
        }
    }

    public interface IQueryExecutor
    {
        object ExecuteScalar<T>(IObjectQuery<T> query, ScalarOptions options) where T : class;

        List<T> ExecuteList<T>(IObjectQuery<T> query, ListOptions options) where T : class;

        IPagedList<T> ExecutePagedList<T>(IObjectQuery<T> query, PaginationOptions options) where T : class;
    }

    public class ScalarOptions
    {
        public string ScalarMethod { get; set; }
    }

    public class ListOptions
    {
        public int Skip { get; set; }

        public int Take { get; set; }

        public Expression Predicate { get; set; }

        public IList<SortDefinition> SortDefinitions { get; set; }
    }

    public class PaginationOptions
    {
        public int PageSize { get; set; }

        public int PageIndex { get; set; }

        public Expression Predicate { get; set; }

        public IList<SortDefinition> SortDefinitions { get; set; }
    }

    public class SortDefinition
    {
        public string Property { get; set; }

        public SortDirection Direction { get; set; }

        public SortDefinition Clone()
        {
            return (SortDefinition)MemberwiseClone();
        }
    }

    public enum SortDirection
    {
        Asc = 0,
        Desc = 1
    }
}
