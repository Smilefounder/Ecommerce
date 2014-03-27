using Kooboo.Commerce.Customers;
using Kooboo.Commerce.Orders;
using Kooboo.Commerce.Products;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;

namespace Kooboo.Commerce.Promotions
{
    public interface IPromotionPolicy
    {
        string Name { get; }

        string DisplayName { get; }

        void Execute(PromotionPolicyExecutionContext context);
    }
}
