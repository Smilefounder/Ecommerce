using Kooboo.CMS.Common.Runtime;
using Kooboo.Commerce.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Kooboo.Commerce.Rules
{
    public class IndirectReferenceParameterValueResolverAdapter : IParameterValueResolver
    {
        private Type _referencingType;
        private Type _referenceResolverType;

        public IndirectReferenceParameterValueResolverAdapter(Type referencingType, Type referenceResolverType)
        {
            _referencingType = referencingType;
            _referenceResolverType = referenceResolverType;
        }

        public object GetValue(ConditionParameter param, object dataContext)
        {
            var referenceResolver = EngineContext.Current.Resolve(_referenceResolverType) as IReferenceResolver;
            return referenceResolver.Resolve(_referencingType, dataContext);
        }
    }
}
