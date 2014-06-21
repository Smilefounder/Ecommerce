using Kooboo.CMS.Common.Runtime.Dependency;
using Kooboo.Commerce.API.Customers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Kooboo.Commerce.CMSIntegration.DataSources.Sources
{
    public class CustomerSource : ApiCommerceSource
    {
        public CustomerSource()
            : base("Customers", typeof(ICustomerQuery))
        {
        }
    }
}