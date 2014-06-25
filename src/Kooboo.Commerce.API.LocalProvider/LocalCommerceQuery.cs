using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Linq.Expressions;
using System.Web;

namespace Kooboo.Commerce.API.LocalProvider
{
    /// <summary>
    /// local commerce query base class
    /// </summary>
    /// <typeparam name="T">api object type</typeparam>
    /// <typeparam name="Model">entity type</typeparam>
    public abstract class LocalCommerceQuery<T, Model> : ICommerceQuery<T>
        where T : class, new()
        where Model : class, new()
    {
        protected IDictionary<string, object> _halParameters;
        protected IMapper<T, Model> _mapper;
        protected List<string> _includeComplexPropertyNames = new List<string>();

        public LocalCommerceQuery(IMapper<T, Model> mapper)
        {
            _mapper = mapper;
            _halParameters = new Dictionary<string, object>();
        }

        /// <summary>
        /// entity query for build up fluent api filters
        /// </summary>
        protected IQueryable<Model> _query;
        /// <summary>
        /// create entity query
        /// </summary>
        /// <returns>queryable object</returns>
        protected abstract IQueryable<Model> CreateQuery();
        /// <summary>
        /// use the default order when pagination the query
        /// </summary>
        /// <param name="query">pagination query</param>
        /// <returns>ordered query</returns>
        protected abstract IQueryable<Model> OrderByDefault(IQueryable<Model> query);
        /// <summary>
        /// map the entity to object
        /// </summary>
        /// <param name="obj">entity</param>
        /// <returns>object</returns>
        protected virtual T Map(Model obj)
        {
            return _mapper.MapTo(obj, _includeComplexPropertyNames.ToArray());
        }
        /// <summary>
        /// this method will be called after query executed
        /// </summary>
        protected virtual void OnQueryExecuted()
        {
            _includeComplexPropertyNames.Clear();
        }

        protected virtual string BuildResourceName(string resourceName)
        {
            return string.Format("{0}:{1}", typeof(T).Name, resourceName).ToLower();
        }

        /// <summary>
        /// ensure the query is not null
        /// </summary>
        protected virtual void EnsureQuery()
        {
            if (_query == null)
                _query = CreateQuery();
        }

        public ICommerceQuery<T> Include(string property)
        {
            if (!_includeComplexPropertyNames.Contains(property))
                _includeComplexPropertyNames.Add(property);
            return this;
        }

        public ICommerceQuery<T> Include<TProperty>(Expression<Func<T, TProperty>> property)
        {
            var expression = property.Body;
            switch (expression.NodeType)
            {
                case ExpressionType.MemberAccess:
                    var member = DecodeMemberExpression(expression as MemberExpression);
                    _includeComplexPropertyNames.AddRange(member.Where(o => !_includeComplexPropertyNames.Contains(o)));
                    break;
                case ExpressionType.Call:
                    var calls = DecodeCallExpression(expression as MethodCallExpression);
                    _includeComplexPropertyNames.AddRange(calls.Where(o => !_includeComplexPropertyNames.Contains(o)));
                    break;
            }
            return this;
        }

        private IEnumerable<string> DecodeMemberExpression(MemberExpression expression)
        {
            List<string> propNames = new List<string>();
            while (expression != null && expression.NodeType != ExpressionType.Parameter)
            {
                var propName = expression.Member.Name;
                for (int i = 0; i < propNames.Count; i++)
                {
                    propNames[i] = string.Format("{0}.{1}", propName, propNames[i]);
                }
                propNames.Insert(0, propName);
                expression = expression.Expression as MemberExpression;
            }
            return propNames;
        }

        private IEnumerable<string> DecodeLambdaExpression(LambdaExpression expression, string prefix)
        {
            if (expression.Body.NodeType == ExpressionType.MemberAccess)
            {
                var props = DecodeMemberExpression(expression.Body as MemberExpression);
                return props.Select(o => string.Format("{0}.{1}", prefix, o));
            }
            else if (expression.Body.NodeType == ExpressionType.Call)
            {
                var props = DecodeCallExpression(expression.Body as MethodCallExpression);
                return props.Select(o => string.Format("{0}.{1}", prefix, o));
            }
            return Enumerable.Empty<string>();
        }

        private IEnumerable<string> DecodeCallExpression(MethodCallExpression expression)
        {
            var paras = new List<string>();

            var args = expression.Arguments;
            if (args.Count > 0 && args[0].NodeType == ExpressionType.MemberAccess)
            {
                var members = DecodeMemberExpression(args[0] as MemberExpression);
                paras.AddRange(members);
                if (args.Count > 1 && args[1].NodeType == ExpressionType.Lambda)
                {
                    var lparas = DecodeLambdaExpression(args[1] as LambdaExpression, members.Last());
                    paras.AddRange(lparas);
                }
            }

            return paras;
        }

        /// <summary>
        /// get paginated data that matches the query
        /// </summary>
        /// <param name="pageIndex">current page index</param>
        /// <param name="pageSize">page size</param>
        /// <returns>paginated data</returns>
        public virtual IList<T> Pagination(int pageIndex, int pageSize)
        {
            EnsureQuery();
            var query = OrderByDefault(_query);
            var objs = query.Skip(pageIndex * pageSize).Take(pageSize).ToArray();
            var mobjs = new List<T>(objs.Select(o => Map(o)).ToArray());
            OnQueryExecuted();

            return mobjs;
        }
        /// <summary>
        /// get the first object that matches the query, if not matched returns null
        /// </summary>
        /// <returns>object</returns>
        public virtual T FirstOrDefault()
        {
            EnsureQuery();
            var obj = _query.FirstOrDefault();
            T mobj = default(T);
            if (obj != null)
                mobj = Map(obj);
            OnQueryExecuted();

            return mobj;
        }
        /// <summary>
        /// get all objects that matches the query
        /// </summary>
        /// <returns>objects</returns>
        public virtual IList<T> ToArray()
        {
            EnsureQuery();
            var objs = _query.ToArray();
            var mobjs = new List<T>(objs.Select(o => Map(o)).ToArray());
            OnQueryExecuted();

            return mobjs;
        }
        /// <summary>
        /// get total hit count that matches the query
        /// </summary>
        /// <returns>total count</returns>
        public virtual int Count()
        {
            EnsureQuery();
            int count = _query.Count();
            return count;
        }
    }
}
