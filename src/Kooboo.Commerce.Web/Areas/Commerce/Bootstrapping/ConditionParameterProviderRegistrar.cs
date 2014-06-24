﻿using Kooboo.CMS.Common.Runtime.Dependency;
using Kooboo.Commerce.Rules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Kooboo.Commerce.Web.Areas.Commerce.Bootstrapping
{
    public class ConditionParameterProviderRegistrar : IDependencyRegistrar
    {
        public int Order
        {
            get
            {
                return 100;
            }
        }

        public void Register(IContainerManager containerManager, CMS.Common.Runtime.ITypeFinder typeFinder)
        {
            foreach (var type in typeFinder.FindClassesOfType<IParameterProvider>())
            {
                if (type != typeof(DeclaringParameterProvider))
                {
                    var provider = Activator.CreateInstance(type) as IParameterProvider;
                    ParameterProviderManager.Instance.Add(provider);
                }
            }
        }
    }
}