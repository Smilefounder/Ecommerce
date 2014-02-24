using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Kooboo.CMS.Common.Runtime.Dependency;
using Kooboo.Commerce.Data;

namespace Kooboo.Commerce.EAV.Services {

    [Dependency(typeof(IFieldValidationRuleService))]
    public class FieldValidationRuleService : IFieldValidationRuleService {

        private readonly IRepository<FieldValidationRule> Repository;
        public FieldValidationRuleService(IRepository<FieldValidationRule> repository) {
            Repository = repository;
        }

        public FieldValidationRule Load(int id) {
            return Repository.Get(o => o.Id == id);
        }

        public IQueryable<FieldValidationRule> Query() {
            return Repository.Query();
        }

        public void Create(FieldValidationRule rule) {
            Repository.Insert(rule);
        }

        public void Update(FieldValidationRule rule) {
            Repository.Update(rule, k => new object[] { k.Id });
        }

        public void Delete(FieldValidationRule rule) {
            Repository.Delete(rule);
        }
    }
}
