using AutoMapper;
using Kooboo.Commerce.Data;
using Kooboo.Commerce.Multilingual.Models;
using Kooboo.Commerce.Multilingual.Storage;
using Kooboo.Commerce.Products;
using Kooboo.Commerce.Web.Framework.Mvc;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Kooboo.Commerce.Multilingual.Controllers
{
    public class ProductTypeController : CommerceController
    {
        private IRepository<ProductType> _repository;
        private ITranslationStore _translationStore;

        public ProductTypeController(IRepository<ProductType> repository)
        {
            _repository = repository;
            _translationStore = TranslationStores.Get(CurrentInstance.Name);
        }

        public ActionResult Index(string culture, int page = 1, int pageSize = 50)
        {
            var models = _repository.Query()
                                    .OrderBy(t => t.Id)
                                    .Paginate(page - 1, pageSize)
                                    .Transform(t => new ProductTypeGridItemModel
                                    {
                                        Id = t.Id,
                                        Name = t.Name
                                    })
                                    .ToPagedList();

            foreach (var model in models)
            {
                var translation = _translationStore.Find(CultureInfo.GetCultureInfo(culture), new EntityKey(typeof(ProductType), model.Id));
                if (translation != null)
                {
                    model.IsTranslated = true;
                    model.IsOutOfDate = translation.IsOutOfDate;
                }
            }

            return View(models);
        }

        public ActionResult OutOfDate(string culture, int page = 1, int pageSize = 50)
        {
            var items = _translationStore.FindOutOfDate(CultureInfo.GetCultureInfo(culture), typeof(ProductType), page - 1, pageSize);
            var ids = items.Select(it => (int)it.EntityKey.Value).ToArray();
            var productTypes = _repository.Query().Where(it => ids.Contains(it.Id)).ToList();

            var models = items.Transform(it =>
            {
                var typeId = (int)it.EntityKey.Value;
                var productType = productTypes.Find(t => t.Id == typeId);
                return new ProductTypeGridItemModel
                {
                    Id = typeId,
                    IsOutOfDate = true,
                    IsTranslated = true,
                    Name = productType.Name
                };
            });

            return View(models.ToPagedList());
        }

        public ActionResult Translate(int id)
        {
            var productType = _repository.Find(id);
            var model = Mapper.Map<ProductType, ProductTypeModel>(productType);
            return View(model);
        }

        [HttpPost, HandleAjaxFormError]
        public ActionResult Translate(int id, string culture, ProductTypeModel model, string @return)
        {
            var entityKey = new EntityKey(typeof(ProductType), id);
            var productType = _repository.Find(model.Id);

            var props = new List<PropertyTranslation>();

            foreach (var field in model.CustomFieldDefinitions)
            {
                var originalField = productType.CustomFieldDefinitions.Find(field.Id);
                UpdateFieldTranslation(originalField, field, "CustomFieldDefinitions[" + field.Name + "].", props);
            }
            foreach (var field in model.VariantFieldDefinitions)
            {
                var originalField = productType.VariantFieldDefinitions.Find(field.Id);
                UpdateFieldTranslation(originalField, field, "VariantFieldDefinitions[" + field.Name + "].", props);
            }

            _translationStore.AddOrUpdate(CultureInfo.GetCultureInfo(culture), entityKey, props);

            return AjaxForm().RedirectTo(@return);
        }

        public ActionResult GetModel(int id, string culture)
        {
            var productType = _repository.Find(id);

            var compared = Mapper.Map<ProductType, ProductTypeModel>(productType);
            var translated = new ProductTypeModel
            {
                Id = compared.Id,
                Name = compared.Name
            };
            
            // Difference of compared product type and the previous compared product type
            var diff = new ProductTypeModel
            {
                Id = compared.Id,
                Name = compared.Name
            };

            var entityKey = EntityKey.FromEntity(productType);
            var translation = _translationStore.Find(CultureInfo.GetCultureInfo(culture), entityKey);

            foreach (var field in compared.CustomFieldDefinitions)
            {
                translated.CustomFieldDefinitions.Add(LoadFieldTranslation(field, "CustomFieldDefinitions", translation));
                diff.CustomFieldDefinitions.Add(LoadFieldDiff(field, "CustomFieldDefinitions", translation));
            }
            foreach (var field in compared.VariantFieldDefinitions)
            {
                translated.VariantFieldDefinitions.Add(LoadFieldTranslation(field, "VariantFieldDefinitions", translation));
                diff.VariantFieldDefinitions.Add(LoadFieldDiff(field, "VariantFieldDefinitions", translation));
            }

            return Json(new
            {
                Compared = compared,
                Difference = diff,
                Translated = translated
            }, JsonRequestBehavior.AllowGet);
        }

        private CustomFieldDefinitionModel LoadFieldTranslation(CustomFieldDefinitionModel original, string prefix, EntityTransaltion translation)
        {
            prefix = prefix + "[" + original.Name + "].";

            var translated = new CustomFieldDefinitionModel
            {
                Id = original.Id,
                Name = original.Name,
                ControlType = original.ControlType
            };

            if (translation != null)
            {
                translated.Label = translation.GetTranslatedText(prefix + "Label");
                translated.DefaultValue = translation.GetTranslatedText(prefix + "DefaultValue");
            }

            foreach (var item in original.SelectionItems)
            {
                var translatedItem = new SelectionItem { Value = item.Value };
                if (translation != null)
                {
                    translatedItem.Text = translation.GetTranslatedText(prefix + "SelectionItems[" + item.Value + "]");
                }
                translated.SelectionItems.Add(item);
            }

            return translated;
        }

        private CustomFieldDefinitionModel LoadFieldDiff(CustomFieldDefinitionModel compared, string prefix, EntityTransaltion translation)
        {
            prefix = prefix + "[" + compared.Name + "].";

            var diff = new CustomFieldDefinitionModel
            {
                Id = compared.Id,
                Name = compared.Name,
                ControlType = compared.ControlType
            };

            if (translation != null)
            {
                diff.Label = DiffHelper.GetDiffHtml(translation.GetOriginalText(prefix + "Label"), compared.Label);
                diff.DefaultValue = DiffHelper.GetDiffHtml(translation.GetOriginalText(prefix + "DefaultValue"), compared.DefaultValue);
            }

            foreach (var item in compared.SelectionItems)
            {
                var itemDiff = new SelectionItem { Value = item.Value };
                if (translation != null)
                {
                    itemDiff.Text = DiffHelper.GetDiffHtml(translation.GetOriginalText(prefix + "SelectionItems[" + item.Value + "]"), item.Text);
                }
                diff.SelectionItems.Add(itemDiff);
            }

            return diff;
        }

        private void UpdateFieldTranslation(CustomFieldDefinition original, CustomFieldDefinitionModel translated, string prefix, List<PropertyTranslation> translations)
        {
            translations.Add(new PropertyTranslation(prefix + "Label", original.Label, translated.Label));
            translations.Add(new PropertyTranslation(prefix + "DefaultValue", original.DefaultValue, translated.DefaultValue));

            foreach (var item in translated.SelectionItems)
            {
                var originalItem = original.SelectionItems.FirstOrDefault(i => i.Value == item.Value);
                translations.Add(new PropertyTranslation(prefix + "SelectionItems[" + item.Value + "]", originalItem.Text, item.Text));
            }
        }
    }
}
