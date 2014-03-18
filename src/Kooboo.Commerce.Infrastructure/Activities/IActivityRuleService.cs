using Kooboo.CMS.Common.Runtime.Dependency;
using Kooboo.Commerce.Data;
using Kooboo.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.Activities
{
    public interface IActivityRuleService
    {
        ActivityRule GetById(int id);

        ActivityRule Create(Type eventType, string conditionsExpression);

        void Delete(ActivityRule rule);

        IEnumerable<ActivityRule> GetRulesByEventType(Type eventType);

        /// <summary>
        /// Ensure the "Always" rule exists for the specified event type.
        /// "Always" rules are the rules always passing and executed.
        /// </summary>
        /// <param name="eventType"></param>
        void EnsureAlwaysRule(Type eventType);
    }

    [Dependency(typeof(IActivityRuleService))]
    public class ActivityRuleService : IActivityRuleService
    {
        private IRepository<ActivityRule> _repository;

        public ActivityRuleService(IRepository<ActivityRule> repository)
        {
            _repository = repository;
        }

        public ActivityRule GetById(int id)
        {
            return _repository.Get(x => x.Id == id);
        }

        public ActivityRule Create(Type eventType, string conditionsExpression)
        {
            var rule = ActivityRule.Create(eventType, conditionsExpression, RuleType.Normal);
            _repository.Insert(rule);
            return rule;
        }

        public void Delete(ActivityRule rule)
        {
            rule.DetachAllActivities();
            _repository.Delete(rule);
        }

        public IEnumerable<ActivityRule> GetRulesByEventType(Type eventType)
        {
            return _repository.Query().ByEvent(eventType).ToList();
        }

        public void EnsureAlwaysRule(Type eventType)
        {
            var eventTypeName = eventType.AssemblyQualifiedNameWithoutVersion();
            if (!_repository.Query(x => x.EventType == eventTypeName && x.Type == RuleType.Always).Any())
            {
                _repository.Insert(ActivityRule.Create(eventType, String.Empty, RuleType.Always));
            }
        }
    }
}
