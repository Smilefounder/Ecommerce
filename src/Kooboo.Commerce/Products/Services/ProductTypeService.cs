using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Kooboo.CMS.Common.Runtime.Dependency;
using Kooboo.Commerce.Data;
using Kooboo.Commerce.Products;
using Kooboo.Commerce.Products.Services;
using Kooboo.Web.Mvc.Paging;
using Kooboo.Commerce.Events;
using Kooboo.Commerce.Events.ProductTypes;

namespace Kooboo.Commerce.Products.Services
{

    [Dependency(typeof(IProductTypeService))]
    public class ProductTypeService : IProductTypeService
    {
        private readonly IRepository<ProductType> _productTypes;
        private readonly IRepository<CustomField> _customFields;

        public ProductTypeService(IRepository<ProductType> productTypes, IRepository<CustomField> customFields)
        {
            _productTypes = productTypes;
            _customFields = customFields;
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
                        type.CustomFields.Add(predefined);
                    }
                    else
                    {
                        type.CustomFields.Add(field);
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
                        type.VariantFields.Add(predefined);
                    }
                    else
                    {
                        type.VariantFields.Add(field);
                    }
                }
            }

            _productTypes.Insert(type);

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
                type.CustomFields.Update(
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

                type.CustomFields.Sort(request.CustomFields.Select(f => f.Name));
            }

            // Variant fields
            if (request.VariantFields != null)
            {
                type.VariantFields.Update(
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

                type.VariantFields.Sort(request.VariantFields.Select(f => f.Name));
            }

            _productTypes.Database.SaveChanges();

            return type;
        }

        public void Delete(ProductType type)
        {
            Disable(type);
            _productTypes.Delete(type);
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