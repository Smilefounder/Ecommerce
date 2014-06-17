using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Kooboo.Commerce.Data;
using Kooboo.CMS.Common.Runtime.Dependency;
using Kooboo.Commerce.Products;

namespace Kooboo.Commerce.EAV.Services
{

    [Dependency(typeof(ICustomFieldService))]
    public class CustomFieldService : ICustomFieldService
    {

        private readonly IRepository<CustomField> repoCustomField;
        private readonly IRepository<FieldValidationRule> repoFieldValidationRule;
        public CustomFieldService(IRepository<CustomField> repoCustomField, IRepository<FieldValidationRule> repoFieldValidationRule)
        {
            this.repoCustomField = repoCustomField;
            this.repoFieldValidationRule = repoFieldValidationRule;
        }

        public CustomField GetById(int id)
        {
            return repoCustomField.Get(id);
        }

        public IQueryable<CustomField> Query()
        {
            return repoCustomField.Query();
        }

        public bool Create(CustomField field)
        {
            return repoCustomField.Insert(field);
        }

        public bool Update(CustomField field)
        {
            return repoCustomField.Update(field, k => new object[] { k.Id });
        }

        public bool Delete(CustomField field)
        {
            return repoCustomField.Delete(field);
        }

        public IEnumerable<CustomField> GetSystemFields()
        {
            return repoCustomField.Query().Where(o => o.FieldType == CustomFieldType.System);
        }
    }
}
