using Kooboo.CMS.Common.Runtime.Dependency;
using Kooboo.Commerce.API.Orders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Kooboo.Commerce.CMSIntegration.DataSources.Sources
{
    public class OrderSource : ApiCommerceSource
    {
        public OrderSource()
            : base("Orders", typeof(IOrderQuery))
        {
        }
    }
}