using Kooboo.Commerce.Customers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;

namespace Kooboo.Commerce.Promotions
{
    // TODO: Rename PromotionCondition entity, or rename this interface to IPromotionConditionProvider?
    public interface IPromotionCondition
    {
        string Name { get; }

        string GetDescription(PromotionCondition condition);

        bool CanMatchProducts { get; }

        ConditionCheckingResult Check(ConditionCheckingContext context);
    }
}
