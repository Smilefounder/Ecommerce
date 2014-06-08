using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace Kooboo.Commerce.API.LocalProvider.Utils
{
    public static class IncludeHelper
    {
        public static IEnumerable<string> GetIncludePaths<T, TProperty>(Expression<Func<T, TProperty>> property)
        {
            var expression = property.Body;
            switch (expression.NodeType)
            {
                case ExpressionType.MemberAccess:
                    return DecodeMemberExpression(expression as MemberExpression);
                case ExpressionType.Call:
                    return DecodeCallExpression(expression as MethodCallExpression);
            }

            return Enumerable.Empty<string>();
        }


        static IEnumerable<string> DecodeMemberExpression(MemberExpression expression)
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

        static IEnumerable<string> DecodeLambdaExpression(LambdaExpression expression, string prefix)
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

        static IEnumerable<string> DecodeCallExpression(MethodCallExpression expression)
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
    }
}
