using Kooboo.CMS.Common.Runtime.Dependency;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.API.HAL
{
    [Dependency(typeof(IContextEnviromentProvider), Key = "All", Order = 0)]
    public class AllContextEnvironementProvider : IContextEnviromentProvider
    {
        public string Name
        {
            get
            {
                return "All";
            }
            set
            {
            }
        }

        public string Description
        {
            get
            {
                return null;
            }
            set
            {
            }
        }

        public bool IsContextRunInEnviroment(HalContext context)
        {
            return true;
        }
    }
}
