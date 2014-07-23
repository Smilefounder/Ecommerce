using Kooboo.Commerce.Categories;
using Kooboo.Commerce.Data;
using Kooboo.Commerce.Globalization;
using Kooboo.Commerce.Multilingual.Models;
using Kooboo.Commerce.Multilingual.Storage;
using Kooboo.Commerce.Web.Framework.Mvc;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Kooboo.Commerce.Multilingual.Controllers
{
    public class CategoryController : CommerceController
    {
        private IRepository<Category> _repository;
        private ITranslationStore _translationStore;

        public CategoryController(IRepository<Category> repository, ITranslationStore translationStore)
        {
            _repository = repository;
            _translationStore = translationStore;
        }

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Translate(string culture, int id)
        {
            var category = _repository.Find(id);

            var compared = new CategoryModel(category);
            var translated = new CategoryModel
            {
                Id = category.Id
            };

            var translation = _translationStore.Find(CultureInfo.GetCultureInfo(culture), new EntityKey(typeof(Category), category.Id));
            if (translation != null)
            {
                translated.Name = translation.Properties["Name"];
                translated.Description = translation.Properties["Description"];
            }

            ViewBag.Compared = compared;

            return View(translated);
        }

        [HttpPost, HandleAjaxFormError]
        public ActionResult Translate(string culture, CategoryModel model, string @return)
        {
            _translationStore.AddOrUpdate(CultureInfo.GetCultureInfo(culture), new EntityKey(typeof(Category), model.Id), new Dictionary<string, string>
            {
                { "Name", model.Name },
                { "Description", model.Description }
            });

            return AjaxForm().RedirectTo(@return);
        }

        public ActionResult Children(int? parentId, int level, string culture)
        {
            var query = _repository.Query();

            if (parentId == null)
            {
                query = query.Where(c => c.Parent == null);
            }
            else
            {
                query = query.Where(c => c.ParentId == parentId);
            }

            var categories = query.Select(c => new CategoryGridItemModel
            {
                Id = c.Id,
                Name = c.Name,
                ChildrenCount = c.Children.Count,
                Level = level
            })
            .OrderBy(c => c.Id)
            .ToList();

            var categoryKeys = categories.Select(c => new EntityKey(typeof(Category), c.Id)).ToArray();
            var translations = _translationStore.Find(CultureInfo.GetCultureInfo(culture), categoryKeys);

            for (var i = 0; i < translations.Length; i++)
            {
                var translation = translations[i];
                if (translation != null)
                {
                    var category = categories[i];
                    category.TranslatedName = translation.Properties["Name"];
                }
            }

            return Json(categories, JsonRequestBehavior.AllowGet);
        }
    }
}
