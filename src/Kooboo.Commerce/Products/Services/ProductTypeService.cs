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

        private readonly ICommerceDatabase _db;
        private readonly IRepository<ProductType> _productTypeRepository;
        private readonly IRepository<CustomField> _customFieldRepository;
        private readonly IRepository<ProductTypeCustomField> _productTypeCustomFieldRepository;
        private readonly IRepository<ProductTypeVariantField> _productTypeVariantFieldRepository;
        private readonly IRepository<FieldValidationRule> _fieldValidationRuleRepository;

        public ProductTypeService(ICommerceDatabase db,
            IRepository<ProductType> productTypeRepository,
            IRepository<CustomField> customFieldRepository,
            IRepository<ProductTypeCustomField> productTypeCustomFieldRepository,
            IRepository<ProductTypeVariantField> productTypeVariantFieldRepository,
            IRepository<FieldValidationRule> fieldValidationRuleRepository)
        {
            _db = db;
            _productTypeRepository = productTypeRepository;
            _customFieldRepository = customFieldRepository;
            _productTypeCustomFieldRepository = productTypeCustomFieldRepository;
            _productTypeVariantFieldRepository = productTypeVariantFieldRepository;
            _fieldValidationRuleRepository = fieldValidationRuleRepository;
        }

        public ProductType GetById(int id)
        {
            return _productTypeRepository.Get(o => o.Id == id);
        }

        public IQueryable<ProductType> Query()
        {
            return _productTypeRepository.Query().OrderBy(t => t.Id);
        }

        public bool Create(ProductType type)
        {
            return _productTypeRepository.Insert(type);
        }

        public bool Update(ProductType type)
        {
            try
            {
                var productTypeCustomFields = _productTypeCustomFieldRepository.Query(o => o.ProductTypeId == type.Id).ToArray();
                _productTypeCustomFieldRepository.SaveAll(_db, productTypeCustomFields, type.CustomFields, (o, n) => o.ProductTypeId == n.ProductTypeId && o.CustomFieldId == n.CustomFieldId,
                    (repo, f) =>
                    {
                        if (f.CustomField != null)
                        {
                            if (f.CustomField.FieldType == CustomFieldType.Custom)
                                _customFieldRepository.Insert(f.CustomField);
                        }
                        repo.Insert(f);
                    },
                    (repo, of, nf) =>
                    {
                    },
                    (repo, f) =>
                    {
                        if (f.CustomFieldId > 0)
                        {
                            _customFieldRepository.DeleteBatch(o => o.Id == f.CustomFieldId && o.FieldType == CustomFieldType.Custom);
                            repo.DeleteBatch(o => o.ProductTypeId == type.Id && o.CustomFieldId == f.CustomFieldId);
                        }
                    });
                var productTypeVariantFields = _productTypeVariantFieldRepository.Query(o => o.ProductTypeId == type.Id).ToArray();
                _productTypeVariantFieldRepository.SaveAll(_db, productTypeVariantFields, type.VariationFields, (o, n) => o.ProductTypeId == n.ProductTypeId && o.CustomFieldId == n.CustomFieldId,
                    (repo, f) =>
                    {
                        if (f.CustomField != null)
                        {
                            if (f.CustomField.FieldType == CustomFieldType.Custom)
                                _customFieldRepository.Insert(f.CustomField);
                        }
                        repo.Insert(f);
                    },
                    (repo, of, nf) =>
                    {
                    },
                    (repo, f) =>
                    {
                        if (f.CustomFieldId > 0)
                        {
                            _customFieldRepository.DeleteBatch(o => o.Id == f.CustomFieldId && o.FieldType == CustomFieldType.Custom);
                            repo.DeleteBatch(o => o.ProductTypeId == type.Id && o.CustomFieldId == f.CustomFieldId);
                        }
                    });
                _productTypeRepository.Update(type, k => new object[] { k.Id });

                type.NotifyUpdated();

                return true;
            }
            catch
            {
                return false;
            }
        }

        //public bool Update(ProductType oldType, ProductType newType)
        //{
        //    oldType.Name = newType.Name;
        //    oldType.SkuAlias = newType.SkuAlias;
        //    oldType.IsEnabled = newType.IsEnabled;
        //    //
        //    if (newType.CustomFields != null)
        //    {
        //        if (oldType.CustomFields == null) { oldType.CustomFields = new List<ProductTypeField>(); }
        //        UpdateCustomFields(oldType.CustomFields, newType.CustomFields);
        //    }
        //    //
        //    if (newType.VariationFields != null)
        //    {
        //        if (oldType.VariationFields == null) { oldType.VariationFields = new List<ProductTypeField>(); }
        //        UpdateCustomFields(oldType.VariationFields, newType.VariationFields);
        //    }
        //    return true;
        //}

        //private void UpdateCustomFields(ICollection<ProductTypeField> oldFields, ICollection<ProductTypeField> newFields)
        //{
        //    var removes = new List<ProductTypeField>(oldFields);
        //    foreach (var item in newFields) {
        //        ProductTypeField field = null;
        //        if (item.CustomFieldId > 0) {
        //            removes = removes.Where(o => o.ProductTypeId == item.ProductTypeId && o.CustomFieldId != item.CustomFieldId).ToList();
        //            field = oldFields.Where(o => o.CustomFieldId == item.CustomFieldId).FirstOrDefault();
        //            if (field != null) {
        //                field
        //                _customFieldRepository.Update(item, k => new object[] { k.Id });
        //                UpdateValidationRules(field.ValidationRules, item.ValidationRules);
        //                continue;
        //            }
        //        }
        //        oldFields.Add(item);
        //        _customFieldRepository.Insert(item);
        //        foreach (var rule in item.ValidationRules) {
        //            _fieldValidationRuleRepository.Insert(rule);
        //        }
        //    }
        //    foreach (var item in removes) {
        //        _customFieldRepository.Delete(item);
        //        foreach (var rule in item.ValidationRules) {
        //            _fieldValidationRuleRepository.Delete(rule);
        //        }
        //    }
        //}

        //private void UpdateValidationRules(ICollection<FieldValidationRule> oldRules, ICollection<FieldValidationRule> newRules)
        //{
        //    var removes = new List<FieldValidationRule>(oldRules);
        //    foreach (var item in newRules)
        //    {
        //        FieldValidationRule rule = null;
        //        if (item.Id > 0)
        //        {
        //            removes = removes.Where(o => o.Id != item.Id).ToList();
        //            rule = oldRules.Where(o => o.Id == item.Id).FirstOrDefault();
        //            if (rule != null)
        //            {
        //                item.CopyTo(rule);
        //                continue;
        //            }
        //        }
        //        _fieldValidationRuleRepository.Insert(item);
        //        oldRules.Add(item);
        //    }
        //    foreach (var item in removes)
        //    {
        //        _fieldValidationRuleRepository.Delete(item);
        //    }
        //}

        public bool Delete(int productTypeId)
        {
            var productType = _productTypeRepository.Get(productTypeId);
            if (productType == null)
            {
                return false;
            }

            return _productTypeRepository.Delete(productType);
        }

        public void Enable(ProductType type)
        {
            if (type.MarkEnabled())
            {
                _db.SaveChanges();
                Event.Raise(new ProductTypeEnabled(type));
            }
        }

        public void Disable(ProductType type)
        {
            if (type.MarkDisabled())
            {
                _db.SaveChanges();
                Event.Raise(new ProductTypeDisabled(type));
            }
        }
    }
}