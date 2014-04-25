using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.API.HAL.Services
{
    public interface IHalRuleService
    {
        HalRule GetById(int id);

        IQueryable<HalRule> Query();

        IQueryable<HalRuleResource> RuleResourceQuery();

        bool Create(HalRule rule);

        bool Update(HalRule rule);
        bool Save(HalRule rule);

        bool Delete(HalRule rule);
    }
}
