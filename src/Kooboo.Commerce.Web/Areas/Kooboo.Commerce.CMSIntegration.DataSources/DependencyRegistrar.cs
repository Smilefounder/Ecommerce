using Kooboo.CMS.Common.Runtime.Dependency;
using Kooboo.Commerce.API.Metadata;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Compilation;

namespace Kooboo.Commerce.CMSIntegration.DataSources
{
    public class DependencyRegistrar : IDependencyRegistrar
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
            // TODO: Use ITypeFinder will throw exception 
            QueryDescriptors.RegisterAssemblies(BuildManager.GetReferencedAssemblies().OfType<Assembly>().ToArray());
        }
    }
}