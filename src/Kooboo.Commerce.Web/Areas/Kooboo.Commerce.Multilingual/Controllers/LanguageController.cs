using Kooboo.Commerce.Multilingual.Storage;
using Kooboo.Commerce.Web.Framework.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Kooboo.Commerce.Multilingual.Controllers
{
    public class LanguageController : CommerceController
    {
        private ILanguageStore _languageStore;

        public LanguageController()
        {
            _languageStore = LanguageStores.Get(CurrentInstance.Name);
        }

        public ActionResult Index()
        {
            var model = _languageStore.All().OrderBy(x => x.Name).ToList();
            return View(model);
        }

        public ActionResult Create()
        {
            return View(new Language());
        }

        [HttpPost, HandleAjaxFormError]
        public ActionResult Create(Language language, string @return)
        {
            _languageStore.Add(language);
            return AjaxForm().RedirectTo(@return);
        }

        public ActionResult Edit(string name)
        {
            ViewBag.IsEditing = true;
            var model = _languageStore.Find(name);
            return View(model);
        }

        [HttpPost, HandleAjaxFormError]
        public ActionResult Edit(Language language, string @return)
        {
            _languageStore.Update(language);
            return AjaxForm().RedirectTo(@return);
        }

        [HttpPost, HandleAjaxFormError]
        public ActionResult Delete(Language[] model)
        {
            foreach (var lang in model)
            {
                _languageStore.Delete(lang.Name);
            }

            return AjaxForm().Success().ReloadPage();
        }
    }
}
