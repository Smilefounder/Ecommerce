using Kooboo.CMS.Common.Runtime;
using Kooboo.Commerce.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Kooboo.Commerce.Rules.Parameters
{
    /// <summary>
    /// 间接引用求解器。当对象之间以对象Id的形式关联时，使用它可以获得该Id关联的实际对象的实例。
    /// </summary>
    public class DefaultIndirectReferenceResolver : IReferenceResolver
    {
        public object Resolve(Type referencingType, object referenceKey)
        {
            var repositoryType = typeof(IRepository<>).MakeGenericType(referencingType);
            var repository = TypeActivator.CreateInstance(repositoryType);
            var candidateMethods = repository.GetType()
                                             .GetMethods(BindingFlags.Public | BindingFlags.Instance)
                                             .Where(m => m.Name == "Find");

            // 找到 IRepository<T>.Find() 方法并调用
            MethodInfo method = null;

            foreach (var candidate in candidateMethods)
            {
                var parameters = candidate.GetParameters();
                if (parameters.Length == 1
                    && parameters[0].ParameterType.IsArray
                    && parameters[0].ParameterType.GetElementType() == typeof(object))
                {
                    method = candidate;
                    break;
                }
            }

            if (method == null)
                throw new InvalidOperationException("Cannot find 'Find' method on IRepository.");

            return method.Invoke(repository, new object[] { new object[] { referenceKey } });
        }
    }
}
