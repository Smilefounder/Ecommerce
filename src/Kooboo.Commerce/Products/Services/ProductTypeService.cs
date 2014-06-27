using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Kooboo.CMS.Common.Runtime.Dependency;
using Kooboo.Commerce.Data;
using Kooboo.Commerce.EAV;
using Kooboo.Commerce.EAV.Services;
using Kooboo.Web.Mvc.Paging;
using Kooboo.Commerce.Events;
using Kooboo.Commerce.Events.ProductTypes;

namespace Kooboo.Commerce.Products.Services
{

    [Dependency(typeof(IProductTypeService))]
    public class ProductTypeService : IProductTypeService
    {
        private readonly IRepository<ProductType> _repository;

        public ProductTypeService(IRepository<ProductType> repository)
        {
            _repository = repository;
        }

        public ProductType GetById(int id)
        {
            return _repository.Find(id);
        }

        public IQueryable<ProductType> Query()
        {
            return _repository.Query().OrderBy(p => p.Id);
        }

        public void Create(ProductType type)
        {
            _repository.Insert(type);
        }

        public void Update(ProductType type)
        {
            var dbType = _repository.Find(type.Id);

            // Basic info
            dbType.Name = type.Name;
            dbType.SkuAlias = type.SkuAlias;

            // Custom fields
            foreach (var field in dbType.CustomFields.ToList())
            {
                if (!type.CustomFields.Any(f => f.CustomFieldId == field.CustomFieldId))
                {
                    dbType.CustomFields.Remove(field);
                }
            }

            foreach (var field in type.CustomFields)
            {
                if (!dbType.CustomFields.Any(f => f.CustomFieldId == field.CustomFieldId))
                {
                    dbType.CustomFields.Add(new ProductTypeCustomField
                    {
                        CustomFieldId = field.CustomFieldId
                    });
                }
            }

            // Variant fields
            foreach (var field in dbType.VariationFields.ToList())
            {
                if (!type.VariationFields.Any(f => f.CustomFieldId == field.CustomFieldId))
                {
                    dbType.VariationFields.Remove(field);
                }
            }

            foreach (var field in type.VariationFields)
            {
                if (!dbType.VariationFields.Any(f => f.CustomFieldId == field.CustomFieldId))
                {
                    dbType.VariationFields.Add(new ProductTypeVariantField
                    {
                        CustomFieldId = field.CustomFieldId
                    });
                }
            }

            _repository.Database.SaveChanges();
        }

        public void Delete(ProductType type)
        {
            Disable(type);
            _repository.Delete(type);
        }

        public bool Enable(ProductType type)
        {
            if (type.IsEnabled)
            {
                return false;
            }

            type.IsEnabled = true;
            _repository.Database.SaveChanges();

            Event.Raise(new ProductTypeEnabled(type));

            return true;
        }

        public bool Disable(ProductType type)
        {
            if (!type.IsEnabled)
            {
                return false;
            }

            type.IsEnabled = false;
            _repository.Database.SaveChanges();

            Event.Raise(new ProductTypeDisabled(type));

            return true;
        }
    }
}