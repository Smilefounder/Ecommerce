using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Kooboo.CMS.Common.Runtime.Dependency;
using Kooboo.Commerce.Data;
using Kooboo.Commerce.EAV;
using Kooboo.Commerce.EAV.Services;
using Kooboo.Web.Mvc.Paging;

namespace Kooboo.Commerce.Products.Services {

    [Dependency(typeof(IProductTypeService))]
    public class ProductTypeService : IProductTypeService {

        private readonly IRepository<ProductType> _productTypeRepository;
        private readonly IRepository<CustomField> _customFieldRepository;
        private readonly IRepository<FieldValidationRule> _fieldValidationRuleRepository;

        public ProductTypeService(IRepository<ProductType> productTypeRepository,
                                  IRepository<CustomField> customFieldRepository,
                                  IRepository<FieldValidationRule> fieldValidationRuleRepository) {
            _productTypeRepository = productTypeRepository;
            _customFieldRepository = customFieldRepository;
            _fieldValidationRuleRepository = fieldValidationRuleRepository;
        }

        public ProductType GetById(int id) {
            ProductType type = _productTypeRepository.Get(o => o.Id == id);
            return type.IsDeleted ? null : type;
        }

        public IEnumerable<ProductType> GetAllProductTypes()
        {
            return _productTypeRepository.Query().Where(x => !x.IsDeleted).ToArray();
        }

        public IPagedList<T> GetAllProductTypes<T>(int? pageIndex, int? pageSize, Func<ProductType, T> func)
        {
            int pi = pageIndex ?? 1;
            int ps = pageSize ?? 50;
            pi = pi < 1 ? 1 : pi;
            ps = ps < 1 ? 50 : ps;
            var query = _productTypeRepository.Query().Where(x => !x.IsDeleted).OrderByDescending(o => o.Id);
            var total = query.Count();
            var data = query.Skip(ps * (pi - 1)).Take(ps).ToArray();
            return new PagedList<T>(data.Select<ProductType, T>(o => func(o)), pi, ps, total);
        }

        public bool Create(ProductType type) {
            return _productTypeRepository.Insert(type);
        }

        public bool Update(ProductType type) {
            return _productTypeRepository.Update(type, k => new object[] { k.Id });
        }

        public bool Update(ProductType oldType, ProductType newType) {
            oldType.Name = newType.Name;
            oldType.SkuAlias = newType.SkuAlias;
            oldType.IsEnabled = newType.IsEnabled;
            //
            if (newType.CustomFields != null) {
                if (oldType.CustomFields == null) { oldType.CustomFields = new List<CustomField>(); }
                UpdateCustomFields(oldType.CustomFields, newType.CustomFields);
            }
            //
            if (newType.VariationFields != null) {
                if (oldType.VariationFields == null) { oldType.VariationFields = new List<CustomField>(); }
                UpdateCustomFields(oldType.VariationFields, newType.VariationFields);
            }
            return true;
        }

        private void UpdateCustomFields(ICollection<CustomField> oldFields, ICollection<CustomField> newFields) {
            var removes = new List<CustomField>(oldFields);
            foreach (var item in newFields) {
                CustomField field = null;
                if (item.Id > 0) {
                    removes = removes.Where(o => o.Id != item.Id).ToList();
                    field = oldFields.Where(o => o.Id == item.Id).FirstOrDefault();
                    if (field != null) {
                        item.CopyTo(field);
                        _customFieldRepository.Update(item, k => new object[] { k.Id });
                        UpdateValidationRules(field.ValidationRules, item.ValidationRules);
                        continue;
                    }
                }
                oldFields.Add(item);
                _customFieldRepository.Insert(item);
                foreach (var rule in item.ValidationRules) {
                    _fieldValidationRuleRepository.Insert(rule);
                }
            }
            foreach (var item in removes) {
                _customFieldRepository.Delete(item);
                foreach (var rule in item.ValidationRules) {
                    _fieldValidationRuleRepository.Delete(rule);
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
                _fieldValidationRuleRepository.Insert(item);
                oldRules.Add(item);
            }
            foreach (var item in removes) {
                _fieldValidationRuleRepository.Delete(item);
            }
        }

        public bool Delete(ProductType type) {
            type.IsDeleted = true;
            if (!type.DeletedAtUtc.HasValue) {
                type.DeletedAtUtc = DateTime.UtcNow;
            }
            return _productTypeRepository.Update(type, k => new object[] { k.Id });
        }

        public void Enable(ProductType type) {
            type.IsEnabled = true;
            _productTypeRepository.Update(type, k => new object[] { k.Id });
        }

        public void Disable(ProductType type) {
            type.IsEnabled = false;
            _productTypeRepository.Update(type, k => new object[] { k.Id });
        }
    }
}