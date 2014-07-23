using Kooboo.Commerce.Brands;
using Kooboo.Commerce.Brands.Services;
using Kooboo.Commerce.Globalization;
using Kooboo.Commerce.Multilingual.Domain;
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

        public BrandController(IBrandService brandService, ITranslationStore translationStore)
        {
            _brandService = brandService;
            _translationStore = translationStore;
        }

        public ActionResult Index(string culture, int page = 1, int pageSize = 50)
        {
            var list = _brandService.Query()
                                    .OrderByDescending(b => b.Id)
                                    .Paginate(page - 1, pageSize)
                                    .Transform(b => new BrandGridItemModel(b))
                                    .ToPagedList();

            var brands = list.ToList();
            var brandKeys = brands.Select(x => new EntityKey(typeof(Brand), x.Id)).ToArray();

            var translations = _translationStore.Find(CultureInfo.GetCultureInfo(culture), brandKeys);
            for (var i = 0; i < translations.Length; i++)
            {
                var brand = brands[i];
                brand.TranslatedName = translations[i].Properties["Name"];
            }

            return View(list);
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


            var translation = _translationStore.Find(CultureInfo.GetCultureInfo(culture), EntityKey.Get<Brand>(brand));
            translation = translation ?? new EntityTransaltion(culture, EntityKey.Get<Brand>(brand));

            var translated = new BrandModel
            {
                Name = translation.Properties["Name"],
                Description = translation.Properties["Description"]
            };

            ViewBag.Compared = compared;

            return View(translated);
        }

        [HttpPost]
        public ActionResult Translate(BrandModel model, string culture, string @return)
        {
            var brandKey = new EntityKey(typeof(Brand), model.Id);
            _translationStore.AddOrUpdate(CultureInfo.GetCultureInfo(culture), brandKey, new Dictionary<string, string>
            {
                { "Name", model.Name },
                { "Description", model.Description }
            });

            return AjaxForm().RedirectTo(@return);
        }
    }
}
