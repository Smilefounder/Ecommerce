using Kooboo.CMS.Common.Runtime.Dependency;
using Kooboo.Commerce.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Kooboo.Commerce.Shipping.ByWeight.Domain.Services
{
    [Dependency(typeof(IByWeightShippingRuleService))]
    public class ByWeightShippingRuleService : IByWeightShippingRuleService
    {
        private IRepository<ByWeightShippingRule> _repository;

        public ByWeightShippingRuleService(IRepository<ByWeightShippingRule> repository)
        {
            _repository = repository;
        }

        public IQueryable<ByWeightShippingRule> Query()
        {
            return _repository.Query();
        }

        public void Create(ByWeightShippingRule rule)
        {
            _repository.Create(rule);
        }

        public void Update(ByWeightShippingRule rule)
        {
            _repository.Update(rule);
        }

        public void Delete(ByWeightShippingRule rule)
        {
            _repository.Delete(rule);
        }
    }
}