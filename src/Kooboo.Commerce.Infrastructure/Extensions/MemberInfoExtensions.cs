using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Kooboo.Commerce
{
    public static class MemberInfoExtensions
    {
        public static T GetCustomAttribute<T>(this MemberInfo member, bool inherit)
            where T : Attribute
        {
            return member.GetCustomAttributes(typeof(T), inherit)
                         .OfType<T>()
                         .FirstOrDefault();
        }
    }
}
