using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Kooboo.Commerce.Data;
using Kooboo.CMS.Common.Runtime.Dependency;
using Kooboo.Commerce.Products;

namespace Kooboo.Commerce.Products.Services
{
    // TODO: Should be predefined custom fields service
    [Dependency(typeof(ICustomFieldService))]
    public class CustomFieldService : ICustomFieldService
    {
        private readonly IRepository<CustomField> repoCustomField;

        public CustomFieldService(IRepository<CustomField> repoCustomField)
        {
            this.repoCustomField = repoCustomField;
        }

        public CustomField GetById(int id)
        {
            return repoCustomField.Find(id);
        }

        public IQueryable<CustomField> Query()
        {
            return repoCustomField.Query();
        }

        public IQueryable<CustomField> PredefinedFields()
        {
            return repoCustomField.Query().Where(o => o.IsPredefined == true);
        }

        public void Create(CustomField field)
        {
            repoCustomField.Insert(field);
        }

        public void Update(CustomField field)
        {
            repoCustomField.Update(field);
        }

        public void Delete(CustomField field)
        {
            repoCustomField.Delete(field);
        }
    }
}
