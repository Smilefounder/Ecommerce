using Kooboo.CMS.Common.Runtime.Dependency;
using Kooboo.Commerce.API.Brands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Kooboo.Commerce.CMSIntegration.DataSources.Sources
{
    public class BrandSource : ApiCommerceSource
    {
        public BrandSource()
            : base("Brands", typeof(IBrandQuery))
        {
        }
    }
}