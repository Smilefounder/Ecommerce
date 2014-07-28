using Kooboo.Commerce.Data;
using Kooboo.Commerce.Globalization;
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
            var fields = productType.CustomFields.Where(f => f.CustomField.IsValueLocalizable).ToList();

            var compared = new ProductModel
            {
                Id = product.Id,
                Name = product.Name
            };

            foreach (var field in fields)
            {
                compared.CustomFields.Add(new CustomFieldValueModel
                {
                    Field = field.CustomField,
                    FieldName = field.CustomField.Name,
                    FieldLabel = field.CustomField.Label,
                    ControlType = field.CustomField.ControlType,
                    FieldValue = product.CustomFields.GetValue(field.CustomField.Name)
                });
            }

            var translated = new ProductModel
            {
                Id = product.Id
            };

            foreach (var field in fields)
            {
                translated.CustomFields.Add(new CustomFieldValueModel
                {
                    Field = field.CustomField,
                    FieldName = field.CustomField.Name,
                    ControlType = field.CustomField.ControlType,
                    FieldLabel = field.CustomField.Label
                });
            }

            var translation = _translationStore.Find(CultureInfo.GetCultureInfo(culture), new EntityKey(typeof(Product), product.Id));
            if (translation != null)
            {
                translated.Name = translation.Properties["Name"];
                foreach (var field in translated.CustomFields)
                {
                    field.FieldValue = translation.Properties["CustomFields[" + field.FieldName + "]"];
                }
            }

            ViewBag.Compared = compared;

            return View(translated);
        }

        [HttpPost, HandleAjaxFormError, ValidateInput(false)]
        public ActionResult Translate(string culture, ProductModel model, string @return)
        {
            var translations = new Dictionary<string, string>();
            translations.Add("Name", model.Name);

            foreach (var field in model.CustomFields)
            {
                translations.Add("CustomFields[" + field.FieldName + "]", field.FieldValue);
            }

            _translationStore.AddOrUpdate(CultureInfo.GetCultureInfo(culture), new EntityKey(typeof(Product), model.Id), translations);

            return AjaxForm().RedirectTo(@return);
        }
    }
}
