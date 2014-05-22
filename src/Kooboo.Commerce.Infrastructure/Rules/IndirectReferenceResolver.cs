using Kooboo.CMS.Common.Runtime;
using Kooboo.Commerce.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Kooboo.Commerce.Rules
{
    public class IndirectReferenceResolver : IReferenceResolver
    {
        public object Resolve(Type referencingType, object referenceKey)
        {
            var repositoryType = typeof(IRepository<>).MakeGenericType(referencingType);
            var repository = EngineContext.Current.Resolve(repositoryType);
            var candidateMethods = repository.GetType()
                                             .GetMethods(BindingFlags.Public | BindingFlags.Instance)
                                             .Where(m => m.Name == "Get");

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

            return method.Invoke(repository, new object[] { new object[] { referenceKey } });
        }
    }
}
