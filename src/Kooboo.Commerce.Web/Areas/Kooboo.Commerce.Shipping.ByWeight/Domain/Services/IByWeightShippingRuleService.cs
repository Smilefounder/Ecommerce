using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Kooboo.Commerce.Shipping.ByWeight.Domain.Services
{
    public interface IByWeightShippingRuleService
    {
        IQueryable<ByWeightShippingRule> Query();

        void Create(ByWeightShippingRule rule);

        void Update(ByWeightShippingRule rule);

        void Delete(ByWeightShippingRule rule);
    }
}