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

        public ActionResult Index(int page = 1, int pageSize = 50)
        {
            var brands = _brandService.Query()
                                      .OrderByDescending(b => b.Id)
                                      .Paginate(page - 1, pageSize)
                                      .ToPagedList();
            return View(brands);
        }

        public ActionResult Translate(int id, string culture)
        {
            var brand = _brandService.GetById(id);
            var compared = new BrandTranslation
            {
                Id = brand.Id,
                Name = brand.Name,
                Description = brand.Description
            };


            var translation = _translationStore.Find(CultureInfo.GetCultureInfo(culture), EntityKey.Get<Brand>(brand))[0];
            translation = translation ?? new EntityTransaltion(culture, EntityKey.Get<Brand>(brand));

            var translated = new BrandTranslation
            {
                Name = translation.PropertyTranslations["Name"],
                Description = translation.PropertyTranslations["Description"]
            };

            ViewBag.Compared = compared;

            return View(translated);
        }

        [HttpPost]
        public ActionResult Translate(BrandTranslation model, string culture, string @return)
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
