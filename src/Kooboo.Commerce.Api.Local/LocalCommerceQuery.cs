using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Linq.Expressions;
using System.Web;
using Kooboo.Commerce.Api.Local;
using Kooboo.Commerce.Api.Local.Mapping;
using System.Globalization;

namespace Kooboo.Commerce.Api.Local
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
        private IncludeCollection _includes = new IncludeCollection();

        protected IncludeCollection Includes
        {
            get
            {
                return _includes;
            }
        }

        private IQueryable<Model> _query;

        protected IQueryable<Model> Query
        {
            get
            {
                if (_query == null)
                {
                    _query = CreateQuery();
                }

                return _query;
            }
            set
            {
                _query = value;
            }
        }

        protected abstract IQueryable<Model> CreateQuery();

        protected abstract IQueryable<Model> OrderByDefault(IQueryable<Model> query);

        protected virtual T Map(Model obj)
        {
            return ObjectMapper.Map<Model, T>(obj, Includes, CultureInfo.CurrentUICulture);
        }

        public ICommerceQuery<T> Include(string property)
        {
            Includes.Add(property);
            return this;
        }

        public ICommerceQuery<T> Include<TProperty>(Expression<Func<T, TProperty>> property)
        {
            var expression = property.Body;
            switch (expression.NodeType)
            {
                case ExpressionType.MemberAccess:
                    var members = DecodeMemberExpression(expression as MemberExpression);
                    _includes.AddRange(members);
                    break;
                case ExpressionType.Call:
                    var calls = DecodeCallExpression(expression as MethodCallExpression);
                    _includes.AddRange(calls);
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

        public virtual IList<T> Pagination(int pageIndex, int pageSize)
        {
            var query = OrderByDefault(Query);
            var models = query.Skip(pageIndex * pageSize).Take(pageSize).ToList();
            return models.Select(m => Map(m)).ToList();
        }

        public virtual T FirstOrDefault()
        {
            var obj = Query.FirstOrDefault();
            if (obj != null)
            {
                return Map(obj);
            }

            return null;
        }

        public virtual IList<T> ToArray()
        {
            return Query.ToList().Select(o => Map(o)).ToList();
        }

        public virtual int Count()
        {
            return Query.Count();
        }
    }
}
