using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Kooboo.Commerce.Data;
using Kooboo.CMS.Common.Runtime.Dependency;
using Kooboo.Commerce.Products;

namespace Kooboo.Commerce.Products.Services
{
    [Dependency(typeof(IPredefinedCustomFieldService))]
    public class PredefinedCustomFieldService : IPredefinedCustomFieldService
    {
        private readonly IRepository<CustomField> _repository;

        public PredefinedCustomFieldService(IRepository<CustomField> repository)
        {
            _repository = repository;
        }

        public CustomField GetById(int id)
        {
            return _repository.Find(id);
        }

        public IQueryable<CustomField> Query()
        {
            return _repository.Query()
                              .Where(o => o.IsPredefined == true)
                              .OrderBy(f => f.Sequence)
                              .ThenBy(f => f.Id);
        }

        public void Create(CustomField field)
        {
            _repository.Insert(field);
        }

        public void Update(CustomField field)
        {
            _repository.Update(field);
        }

        public void UpdateWith(IEnumerable<CustomField> newFields)
        {
            var oldFields = Query().ToList();

            oldFields.Update(
                from: newFields,
                by: f => f.Id,
                onUpdateItem: (oldField, newField) => oldField.UpdateFrom(newField),
                onAddItem: (item) => _repository.Insert(item),
                onRemoveItem: (item) => _repository.Delete(item));
        }

        public void Delete(CustomField field)
        {
            _repository.Delete(field);
        }
    }
}
