using Kooboo.Commerce.Brands;
using Kooboo.Commerce.Brands.Services;
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
    public class BrandController : CommerceController
    {
        private IBrandService _brandService;
        private ITranslationStore _translationStore;

        public BrandController(IBrandService brandService)
        {
            _brandService = brandService;
            _translationStore = TranslationStores.Get(CurrentInstance.Name);
        }

        public ActionResult Index(string culture, string search, int page = 1, int pageSize = 50)
        {
            var query = _brandService.Query();

            if (!String.IsNullOrWhiteSpace(search))
            {
                search = search.Trim();
                query = query.Where(b => b.Name.Contains(search));
            }

            var list = query.OrderByDescending(b => b.Id)
                            .Paginate(page - 1, pageSize)
                            .Transform(b => new BrandGridItemModel(b))
                            .ToPagedList();

            var brands = list.ToList();
            var brandKeys = brands.Select(x => new EntityKey(typeof(Brand), x.Id)).ToArray();

            var translations = _translationStore.Find(CultureInfo.GetCultureInfo(culture), brandKeys);
            for (var i = 0; i < translations.Length; i++)
            {
                if (translations[i] != null)
                {
                    var brand = brands[i];
                    brand.TranslatedName = translations[i].GetTranslatedText("Name");
                    brand.IsTranslated = true;
                    brand.IsOutOfDate = translations[i].IsOutOfDate;
                }
            }

            return View(list);
        }

        public ActionResult OutOfDate(string culture, int page = 1, int pageSize = 50)
        {
            var items = _translationStore.FindOutOfDate(CultureInfo.GetCultureInfo(culture), typeof(Brand), page - 1, pageSize);
            var brandIds = items.Select(t => (int)t.EntityKey.Value).ToArray();
            var brands = _brandService.Query().Where(b => brandIds.Contains(b.Id)).ToList();

            var models = items.Transform(t =>
            {
                var brandId = (int)t.EntityKey.Value;
                var brand = brands.Find(b => b.Id == brandId);
                return new BrandGridItemModel(brand)
                {
                    IsOutOfDate = t.IsOutOfDate,
                    IsTranslated = true,
                    TranslatedName = t.GetTranslatedText("Name")
                };
            });

            return View(models.ToPagedList());
        }

        public ActionResult Translate(int id, string culture)
        {
            var brand = _brandService.GetById(id);
            var compared = new BrandModel
            {
                Id = brand.Id,
                Name = brand.Name,
                Description = brand.Description
            };

            var translated = new BrandModel
            {
                Id = brand.Id
            };

            var diff = new BrandModel
            {
                Id = brand.Id
            };

            var translation = _translationStore.Find(CultureInfo.GetCultureInfo(culture), EntityKey.Get(brand));
            if (translation != null)
            {
                translated.Name = translation.GetTranslatedText("Name");
                translated.Description = translation.GetTranslatedText("Description");

                diff.Name = DiffHelper.GetDiffHtml(translation.GetOriginalText("Name"), brand.Name);
                diff.Description = DiffHelper.GetDiffHtml(translation.GetOriginalText("Description"), brand.Description);
            }

            ViewBag.Difference = diff;
            ViewBag.Compared = compared;

            return View(translated);
        }

        [HttpPost]
        public ActionResult Translate(BrandModel model, string culture, string @return)
        {
            var brandKey = new EntityKey(typeof(Brand), model.Id);
            var brand = _brandService.GetById(model.Id);

            var props = new List<PropertyTranslation>
            {
                new PropertyTranslation("Name", brand.Name, model.Name),
                new PropertyTranslation("Description", brand.Description, model.Description)
            };

            _translationStore.AddOrUpdate(CultureInfo.GetCultureInfo(culture), brandKey, props);

            return AjaxForm().RedirectTo(@return);
        }
    }
}
