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

        public ProductTypeController(IRepository<ProductType> repository, ITranslationStore translationStore)
        {
            _repository = repository;
            _translationStore = translationStore;
        }

        public ActionResult Index(int page = 1, int pageSize = 50)
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
            return View(models);
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
            var translation = _translationStore.Find(CultureInfo.GetCultureInfo(culture), entityKey) ?? new EntityTransaltion(culture, entityKey);

            foreach (var field in model.CustomFieldDefinitions)
            {
                UpdateFieldTranslation(field, "CustomFieldDefinitions[" + field.Name + "].", translation);
            }
            foreach (var field in model.VariantFieldDefinitions)
            {
                UpdateFieldTranslation(field, "VariantFieldDefinitions[" + field.Name + "].", translation);
            }

            _translationStore.AddOrUpdate(CultureInfo.GetCultureInfo(culture), entityKey, translation.Properties);

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

            var entityKey = EntityKey.Get(productType);
            var translation = _translationStore.Find(CultureInfo.GetCultureInfo(culture), entityKey) ?? new EntityTransaltion(culture, entityKey);

            foreach (var field in compared.CustomFieldDefinitions)
            {
                translated.CustomFieldDefinitions.Add(LoadFieldTranslation(field, "CustomFieldDefinitions", translation));
            }

            foreach (var field in compared.VariantFieldDefinitions)
            {
                translated.VariantFieldDefinitions.Add(LoadFieldTranslation(field, "VariantFieldDefinitions", translation));
            }

            return Json(new
            {
                Compared = compared,
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
                ControlType = original.ControlType,
                Label = translation.Properties[prefix + "Label"],
                DefaultValue = translation.Properties[prefix + "DefaultValue"]
            };

            foreach (var item in original.SelectionItems)
            {
                translated.SelectionItems.Add(new SelectionItem
                {
                    Text = translation.Properties[prefix + "SelectionItems[" + item.Value + "]"],
                    Value = item.Value
                });
            }

            return translated;
        }

        private void UpdateFieldTranslation(CustomFieldDefinitionModel translated, string prefix, EntityTransaltion translation)
        {
            translation.Properties[prefix + "Label"] = translated.Label;
            translation.Properties[prefix + "DefaultValue"] = translated.DefaultValue;

            foreach (var item in translated.SelectionItems)
            {
                translation.Properties[prefix + "SelectionItems[" + item.Value + "]"] = item.Text;
            }
        }
    }
}
