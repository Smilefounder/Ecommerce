using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Kooboo.CMS.Common.Runtime.Dependency;
using Kooboo.Commerce.Data;

namespace Kooboo.Commerce.API.HAL.Services
{
    [Dependency(typeof(IHalRuleService))]
    public class HalRuleService : IHalRuleService
    {
        private readonly ICommerceDatabase _db;
        private readonly IRepository<HalRule> _ruleRepository;
        private readonly IRepository<HalRuleResource> _ruleResourceRepository;
        public HalRuleService(ICommerceDatabase db, IRepository<HalRule> ruleRepository, IRepository<HalRuleResource> ruleResourceRepository)
        {
            _db = db;
            _ruleRepository = ruleRepository;
            _ruleResourceRepository = ruleResourceRepository;
        }

        public HalRule GetById(int id)
        {
            var query = Query();
            return query.Where(o => o.Id == id).FirstOrDefault();
        }

        public IQueryable<HalRule> Query()
        {
            var query = _ruleRepository.Query();
            query = query.Include(o => o.Resources);
            return query;
        }

        public IQueryable<HalRuleResource> RuleResourceQuery()
        {
            return _ruleResourceRepository.Query();
        }

        public bool Create(HalRule rule)
        {
            return _ruleRepository.Insert(rule);
        }

        public bool Update(HalRule rule)
        {
            try
            {
                _ruleRepository.Update(rule, k => new object[] { k.Id });
                _ruleResourceRepository.DeleteBatch(o => o.RuleId == rule.Id);
                if (rule.Resources != null && rule.Resources.Count > 0)
                {
                    foreach (var cf in rule.Resources)
                    {
                        _ruleResourceRepository.Insert(cf);
                    }
                }
                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool Save(HalRule rule)
        {
            if (rule.Id > 0)
            {
                bool exists = _ruleRepository.Query(o => o.Id == rule.Id).Any();
                if (exists)
                    return Update(rule);
                else
                    return Create(rule);
            }
            else
            {
                return Create(rule);
            }
        }

        public bool Delete(HalRule rule)
        {
            try
            {
                _ruleResourceRepository.DeleteBatch(o => o.RuleId == rule.Id);
                _ruleRepository.Delete(rule);
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
