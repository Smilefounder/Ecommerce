using Kooboo.CMS.Common.Runtime.Dependency;
using Kooboo.Commerce.Data;
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
            var rule = ActivityRule.Create(eventType, conditionsExpression);
            _repository.Insert(rule);
            return rule;
        }

        public void Delete(ActivityRule rule)
        {
            foreach (var activity in rule.AttachedActivities)
            {
                rule.DetacheActivity(activity.Id);
            }

            _repository.Delete(rule);
        }

        public IEnumerable<ActivityRule> GetRulesByEventType(Type eventType)
        {
            var eventTypeName = eventType.GetVersionUnawareAssemblyQualifiedName();
            return _repository.Query(x => x.EventType == eventTypeName)
                              .OrderBy(x => x.Sequence)
                              .ThenBy(x => x.Id)
                              .ToList();
        }
    }
}
