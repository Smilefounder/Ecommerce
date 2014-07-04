﻿using Kooboo.CMS.Common.Runtime;
using Kooboo.Commerce.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Kooboo.Commerce.Rules.Parameters
{
    /// <summary>
    /// 当计算通过Id间接引用的对象中的参数值时，需要先获得引用对象的实例，
    /// 本类可以提供一个适配，将<see cref="Kooboo.Commerce.Rules.IReferenceResolver"/>适配为一个参数值求解器，
    /// 进而可以在参数求值链将对间接引用的对象进行衔接。
    /// </summary>
    public class IndirectReferenceAdapter : RuleParameterValueResolver
    {
        private Type _referencingType;
        private Type _referenceResolverType;

        public IndirectReferenceAdapter(Type referencingType, Type referenceResolverType)
        {
            _referencingType = referencingType;
            _referenceResolverType = referenceResolverType;
        }

        public override object ResolveValue(RuleParameter param, object dataContext)
        {
            var referenceResolver = TypeActivator.CreateInstance(_referenceResolverType) as IReferenceResolver;
            if (referenceResolver == null)
                throw new InvalidOperationException("Cannot resolve reference resolver type: " + _referenceResolverType + ".");

            return referenceResolver.Resolve(_referencingType, dataContext);
        }
    }
}
