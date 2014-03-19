using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace Kooboo.Commerce.API.Expressions
{
    public static class PropertyPathBuilder
    {
        public static string BuildPropertyPath<T, TKey>(Expression<Func<T, TKey>> property)
        {
            var memberExp = property.Body as MemberExpression;
            if (memberExp == null)
                throw new ArgumentException("Requires member access expression.", "property");

            return memberExp.Member.Name;
        }
    }
}
