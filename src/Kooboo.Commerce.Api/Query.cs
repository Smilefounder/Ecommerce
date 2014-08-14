using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace Kooboo.Commerce.Api
{
    public class Query<T>
        where T : class
    {
        private IQueryExecutor<T> _executor;

        public Dictionary<string, object> Options { get; private set; }

        public List<QueryFilter> Filters { get; private set; }

        public List<Sort> Sorts { get; private set; }

        public IncludeCollection Includes { get; private set; }

        public int Start { get; private set; }

        public int Limit { get; private set; }

        public Query(IQueryExecutor<T> executor)
        {
            _executor = executor;
            Options = new Dictionary<string, object>();
            Filters = new List<QueryFilter>();
            Sorts = new List<Sort>();
            Includes = new IncludeCollection();
            Limit = Int32.MaxValue;
        }

        public Query<T> AddFilter(string name, object parameters)
        {
            return AddFilter(new QueryFilter(name, parameters));
        }

        public Query<T> AddFilter(string name, IDictionary<string, object> parameters)
        {
            return AddFilter(new QueryFilter(name, parameters));
        }

        public Query<T> AddFilter(QueryFilter filter)
        {
            Filters.Add(filter);
            return this;
        }

        public Query<T> Include(string path)
        {
            Includes.Add(path);
            return this;
        }

        public Query<T> Include<TProperty>(Expression<Func<T, TProperty>> property)
        {
            var expression = property.Body;
            switch (expression.NodeType)
            {
                case ExpressionType.MemberAccess:
                    var members = DecodeMemberExpression(expression as MemberExpression);
                    Includes.AddRange(members);
                    break;
                case ExpressionType.Call:
                    var calls = DecodeCallExpression(expression as MethodCallExpression);
                    Includes.AddRange(calls);
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

        public Query<T> Skip(int count)
        {
            Start = count;
            return this;
        }

        public Query<T> Take(int count)
        {
            Limit = count;
            return this;
        }

        public int Count()
        {
            return _executor.Count(this);
        }

        public T FirstOrDefault()
        {
            return _executor.FirstOrDefault(this);
        }

        public List<T> ToList()
        {
            return _executor.ToList(this);
        }
    }
}
