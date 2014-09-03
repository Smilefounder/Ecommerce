using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Kooboo.CMS.Common.Runtime.Dependency;
using Kooboo.Commerce.Data;
using Kooboo.Commerce.Products;
using Kooboo.Commerce.Events;
using Kooboo.Commerce.Events.ProductTypes;

namespace Kooboo.Commerce.Products
{
    [Dependency(typeof(ProductTypeService))]
    public class ProductTypeService
    {
        private readonly IRepository<ProductType> _productTypes;
        private readonly IRepository<CustomFieldDefinition> _customFields;

        public ProductTypeService(ICommerceDatabase database)
        {
            _productTypes = database.GetRepository<ProductType>();
            _customFields = database.GetRepository<CustomFieldDefinition>();
        }

        public ProductType GetById(int id)
        {
            return _productTypes.Find(id);
        }

        public IQueryable<ProductType> Query()
        {
            return _productTypes.Query().OrderBy(p => p.Id);
        }

        public ProductType Create(CreateProductTypeRequest request)
        {
            var type = new ProductType
            {
                Name = request.Name,
                SkuAlias = request.SkuAlias
            };

            var predefinedFields = _customFields.Query().Where(f => f.IsPredefined).ToList();

            if (request.CustomFields != null)
            {
                foreach (var field in request.CustomFields)
                {
                    if (field.IsPredefined)
                    {
                        var predefined = predefinedFields.Find(f => f.Id == field.Id);
                        type.CustomFieldDefinitions.Add(predefined);
                    }
                    else
                    {
                        type.CustomFieldDefinitions.Add(field);
                    }
                }
            }

            if (request.VariantFields != null)
            {
                foreach (var field in request.VariantFields)
                {
                    if (field.IsPredefined)
                    {
                        var predefined = predefinedFields.Find(f => f.Id == field.Id);
                        type.VariantFieldDefinitions.Add(predefined);
                    }
                    else
                    {
                        type.VariantFieldDefinitions.Add(field);
                    }
                }
            }

            _productTypes.Insert(type);

            Event.Raise(new ProductTypeCreated(type));

            return type;
        }

        public ProductType Update(UpdateProductTypeRequest request)
        {
            var type = _productTypes.Find(request.Id);

            // Basic info
            type.Name = request.Name;
            type.SkuAlias = request.SkuAlias;

            // Custom fields
            if (request.CustomFields != null)
            {
                RefreshPredefinedFields(request.CustomFields);

                type.CustomFieldDefinitions.Update(
                    from: request.CustomFields,
                    by: f => f.Id,
                    onUpdateItem: (oldItem, newItem) =>
                    {
                        oldItem.UpdateFrom(newItem);
                    },
                    onRemoveItem: field =>
                    {
                        if (!field.IsPredefined)
                        {
                            _customFields.Delete(field);
                        }
                    });

                type.CustomFieldDefinitions.Sort(request.CustomFields.Select(f => f.Name));
            }

            // Variant fields
            if (request.VariantFields != null)
            {
                RefreshPredefinedFields(request.VariantFields);

                type.VariantFieldDefinitions.Update(
                    from: request.VariantFields,
                    by: f => f.Id,
                    onUpdateItem: (oldItem, newItem) =>
                    {
                        oldItem.UpdateFrom(newItem);
                    },
                    onRemoveItem: field =>
                    {
                        if (!field.IsPredefined)
                        {
                            _customFields.Delete(field);
                        }
                    });

                type.VariantFieldDefinitions.Sort(request.VariantFields.Select(f => f.Name));
            }

            _productTypes.Database.SaveChanges();

            Event.Raise(new ProductTypeUpdated(type));

            return type;
        }

        private void RefreshPredefinedFields(List<CustomFieldDefinition> fields)
        {
            for (var i = 0; i < fields.Count; i++)
            {
                if (fields[i].IsPredefined)
                {
                    fields[i] = _customFields.Find(fields[i].Id);
                }
            }
        }

        public void Delete(ProductType type)
        {
            Disable(type);
            _productTypes.Delete(type);
            Event.Raise(new ProductTypeDeleted(type));
        }

        public bool Enable(ProductType type)
        {
            if (type.IsEnabled)
            {
                return false;
            }

            type.IsEnabled = true;
            _productTypes.Database.SaveChanges();

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
            _productTypes.Database.SaveChanges();

            Event.Raise(new ProductTypeDisabled(type));

            return true;
        }
    }
}