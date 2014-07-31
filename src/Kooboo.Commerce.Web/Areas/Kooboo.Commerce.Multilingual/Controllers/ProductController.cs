using AutoMapper;
using Kooboo.Commerce.Data;
using Kooboo.Commerce.Globalization;
using Kooboo.Commerce.Multilingual.Models;
using Kooboo.Commerce.Multilingual.Storage;
using Kooboo.Commerce.Products;
using Kooboo.Commerce.Web.Framework.Mvc;
using Kooboo.Commerce.Web.Framework.UI.Form;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Kooboo.Commerce.Multilingual.Controllers
{
    public class ProductController : CommerceController
    {
        private IServiceFactory _services;
        private ITranslationStore _translationStore;

        public ProductController(IServiceFactory serviceFactory, ITranslationStore translationStore)
        {
            _services = serviceFactory;
            _translationStore = translationStore;
        }

        public ActionResult Index(string culture, string search, int page = 1, int pageSize = 50)
        {
            var query = _services.Products.Query();
            if (!String.IsNullOrWhiteSpace(search))
            {
                search = search.Trim();
                query = query.Where(p => p.Name.Contains(search));
            }

            var list = query.OrderByDescending(p => p.Id)
                            .Paginate(page - 1, pageSize)
                            .Transform(p => new ProductGridItemModel
                            {
                                Id = p.Id,
                                Name = p.Name
                            })
                            .ToPagedList();

            var products = list.ToList();
            var productKeys = products.Select(p => new EntityKey(typeof(Product), p.Id)).ToArray();
            var translations = _translationStore.Find(CultureInfo.GetCultureInfo(culture), productKeys);

            for (var i = 0; i < translations.Length; i++)
            {
                var translation = translations[i];
                if (translation != null)
                {
                    var product = products[i];
                    product.TranslatedName = translation.Properties["Name"];
                }
            }

            return View(list);
        }

        public ActionResult Translate(int id, string culture)
        {
            var product = _services.Products.GetById(id);
            var productType = _services.ProductTypes.GetById(product.ProductTypeId);

            var controls = FormControls.Controls().ToList();

            var compared = Mapper.Map<Product, ProductModel>(product);
            var translated = Mapper.Map<Product, ProductModel>(product);

            var productKey = new EntityKey(typeof(Product), product.Id);
            var translation = _translationStore.Find(CultureInfo.GetCultureInfo(culture), productKey) ?? new EntityTransaltion(culture, productKey);

            translated.Name = translation.Properties["Name"];

            // Custom fields
            // Product type definition might change, so we need to display fields base on product type definition
            compared.CustomFields.Clear();
            translated.CustomFields.Clear();

            foreach (var definition in productType.CustomFieldDefinitions)
            {
                var control = controls.Find(c => c.Name == definition.ControlType);
                var field = product.CustomFields.FirstOrDefault(f => f.FieldName == definition.Name) ?? new ProductCustomField(definition.Name, null);

                var fieldModel = Mapper.Map<CustomFieldModel>(field);
                fieldModel.FieldText = control.GetFieldDisplayText(definition, field.FieldValue);

                compared.CustomFields.Add(fieldModel);

                fieldModel = Mapper.Map<CustomFieldModel>(field);
                fieldModel.FieldText = control.GetFieldDisplayText(definition, field.FieldValue);

                // If the field value is defined in product editing page, then it's always the field value that get translated
                if (!control.IsSelectionList && !control.IsValuesPredefined)
                {
                    fieldModel.FieldText = translation.Properties["CustomFields[" + field.FieldName + "]"];
                    fieldModel.FieldValue = fieldModel.FieldText;
                }

                translated.CustomFields.Add(fieldModel);
            }

            // Variants
            translated.Variants = GetVariants(product, productType, culture);

            ViewBag.ProductType = productType;
            ViewBag.Compared = compared;

            return View(translated);
        }

        [HttpPost, HandleAjaxFormError, ValidateInput(false)]
        public ActionResult Translate(string culture, ProductModel model, string @return)
        {
            var product = _services.Products.GetById(model.Id);
            var productType = _services.ProductTypes.GetById(product.ProductTypeId);
            var controls = FormControls.Controls().ToList();

            var translations = new Dictionary<string, string>();
            translations.Add("Name", model.Name);

            foreach (var definition in productType.CustomFieldDefinitions)
            {
                var field = model.CustomFields.FirstOrDefault(c => c.FieldName == definition.Name);
                var control = controls.Find(c => c.Name == definition.ControlType);

                if (!control.IsSelectionList && !control.IsValuesPredefined)
                {
                    translations.Add("CustomFields[" + field.FieldName + "]", field.FieldValue);
                }
            }

            _translationStore.AddOrUpdate(CultureInfo.GetCultureInfo(culture), new EntityKey(typeof(Product), model.Id), translations);

            foreach (var variant in model.Variants)
            {
                var variantKey = new EntityKey(typeof(ProductVariant), variant.Id);
                _translationStore.AddOrUpdate(CultureInfo.GetCultureInfo(culture), variantKey, GetVariantTranslations(variant, productType, controls));
            }

            return AjaxForm().RedirectTo(@return);
        }

        private IDictionary<string, string> GetVariantTranslations(ProductVariantModel model, ProductType productType, List<IFormControl> controls)
        {
            var translations = new Dictionary<string, string>();
            foreach (var definition in productType.VariantFieldDefinitions)
            {
                var field = model.VariantFields.FirstOrDefault(f => f.Translated.FieldName == definition.Name);
                var control = controls.Find(c => c.Name == definition.ControlType);
                if (!control.IsSelectionList && !control.IsValuesPredefined)
                {
                    translations.Add("VariantFields[" + definition.Name + "]", field.Translated.FieldValue);
                }
            }

            return translations;
        }

        private List<ProductVariantModel> GetVariants(Product product, ProductType productType, string culture)
        {
            var controls = FormControls.Controls().ToList();
            var models = new List<ProductVariantModel>();

            foreach (var variant in product.Variants)
            {
                var variantKey = new EntityKey(typeof(ProductVariant), variant.Id);
                var translation = _translationStore.Find(CultureInfo.GetCultureInfo(culture), variantKey) ?? new EntityTransaltion(culture, variantKey);

                var model = new ProductVariantModel
                {
                    Id = variant.Id,
                    Sku = variant.Sku,
                    Price = variant.Price
                };

                foreach (var fieldDefinition in productType.VariantFieldDefinitions)
                {
                    // field might be null because admin can add new fields to product types when products already exist
                    var field = variant.VariantFields.FirstOrDefault(f => f.FieldName == fieldDefinition.Name);
                    var control = controls.Find(c => c.Name == fieldDefinition.ControlType);
                    var compared = new CustomFieldModel
                    {
                        FieldName = fieldDefinition.Name,
                        FieldText = control.GetFieldDisplayText(fieldDefinition, field == null ? null : field.FieldValue),
                        FieldValue = field == null ? null : field.FieldValue
                    };

                    var translated = new CustomFieldModel
                    {
                        FieldName = compared.FieldName,
                        FieldText = compared.FieldText,
                        FieldValue = compared.FieldValue
                    };

                    // If the field value is entered in product editing page, then it's always the field value that get translated
                    if (!control.IsSelectionList && !control.IsValuesPredefined)
                    {
                        translated.FieldText = translation.Properties["VariantFields[" + fieldDefinition.Name + "]"];
                        translated.FieldValue = translated.FieldText;
                    }

                    model.VariantFields.Add(new TranslationPair<CustomFieldModel>(compared, translated));
                }

                models.Add(model);
            }

            return models;
        }
    }
}
