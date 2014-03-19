using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Kooboo.Commerce.Data;
using Kooboo.CMS.Common.Runtime.Dependency;
using Kooboo.Commerce.Products;

namespace Kooboo.Commerce.EAV.Services {

    [Dependency(typeof(ICustomFieldService))]
    public class CustomFieldService : ICustomFieldService {

        private readonly IRepository<CustomField> repoCustomField;
        private readonly IRepository<FieldValidationRule> repoFieldValidationRule;
        public CustomFieldService(IRepository<CustomField> repoCustomField, IRepository<FieldValidationRule> repoFieldValidationRule)
        {
            this.repoCustomField = repoCustomField;
            this.repoFieldValidationRule = repoFieldValidationRule;
        }

        public CustomField Load(int id) {
            return repoCustomField.Get(o => o.Id == id);
        }

        public IQueryable<CustomField> Query() {
            return repoCustomField.Query();
        }

        public bool Create(CustomField field) {
            return repoCustomField.Insert(field);
        }

        public bool Update(CustomField field) {
            return repoCustomField.Update(field, k => new object[] { k.Id });
        }

        public bool Delete(CustomField field) {
            return repoCustomField.Delete(field);
        }

        public IEnumerable<CustomField> GetSystemFields() {
            return repoCustomField.Query().Where(o => o.FieldType == CustomFieldType.System);
        }

        public void SetSystemFields(IEnumerable<CustomField> fields) {
            if (fields == null) {
                var list = GetSystemFields();
                foreach (var item in list) {
                    repoCustomField.Delete(item);
                }
            } else {
                var removes = this.GetSystemFields().ToList();
                foreach (var item in fields) {
                    item.FieldType = CustomFieldType.System;
                    CustomField field = null;
                    if (item.Id > 0) {
                        removes = removes.Where(o => o.Id != item.Id).ToList();
                        field = repoCustomField.Get(o => o.Id == item.Id);
                        if (field != null) {
                            item.CopyTo(field);
                            repoCustomField.Update(field, k => new object[] { k.Id });
                            UpdateValidationRules(field.ValidationRules, item.ValidationRules);
                            continue;
                        }
                    }
                    field = new CustomField();
                    item.CopyTo(field);
                    repoCustomField.Insert(field);
                    foreach (var rule in item.ValidationRules) {
                        repoFieldValidationRule.Insert(rule);
                    }
                }
                foreach (var item in removes) {
                    repoCustomField.Delete(item);
                    foreach (var rule in item.ValidationRules) {
                        repoFieldValidationRule.Delete(rule);
                    }
                }
            }
        }

        private void UpdateValidationRules(ICollection<FieldValidationRule> oldRules, ICollection<FieldValidationRule> newRules) {
            var removes = new List<FieldValidationRule>(oldRules);
            foreach (var item in newRules) {
                FieldValidationRule rule = null;
                if (item.Id > 0) {
                    removes = removes.Where(o => o.Id != item.Id).ToList();
                    rule = oldRules.Where(o => o.Id == item.Id).FirstOrDefault();
                    if (rule != null) {
                        item.CopyTo(rule);
                        continue;
                    }
                }
                repoFieldValidationRule.Insert(item);
                oldRules.Add(item);
            }
            foreach (var item in removes) {
                repoFieldValidationRule.Delete(item);
            }
        }
    }
}
