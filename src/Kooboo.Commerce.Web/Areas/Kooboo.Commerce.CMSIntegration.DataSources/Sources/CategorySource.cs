using Kooboo.CMS.Common.Runtime.Dependency;
using Kooboo.Commerce.API.Categories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Kooboo.Commerce.CMSIntegration.DataSources.Sources
{
    public class CategorySource : ApiCommerceSource
    {
        public CategorySource()
            : base("Categories", typeof(ICategoryQuery))
        {
        }
    }
}