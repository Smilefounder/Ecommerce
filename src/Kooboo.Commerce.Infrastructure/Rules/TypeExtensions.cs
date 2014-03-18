using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.Rules
{
    public static class TypeExtensions
    {
        static readonly HashSet<Type> _numberTypes = new HashSet<Type>
        {
            typeof(float), typeof(double), typeof(decimal),
            typeof(short), typeof(int), typeof(long),
            typeof(ushort), typeof(uint), typeof(ulong)
        };

        public static bool IsNumber(this Type type)
        {
            Require.NotNull(type, "type");
            return _numberTypes.Contains(type);
        }
    }
}
