using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Kooboo.CMS.Common;
using Kooboo.Commerce.Data;
using Kooboo.Commerce.Countries;
using Kooboo.Commerce.Web.Areas.Commerce.Models.Countries;
using Kooboo.Commerce.Web.Framework.Mvc;

namespace Kooboo.Commerce.Web.Areas.Commerce.Controllers
{
    public class CountryController : CommerceController
    {
        private readonly CountryService _countryService;

        public CountryController(CountryService countryService)
        {
            _countryService = countryService;
        }

        public ActionResult Index(int page = 1, int pageSize = 50)
        {
            var bindings = _countryService.Query()
                                          .OrderBy(o => o.Name)
                                          .Paginate(page - 1, pageSize)
                                          .Transform(o => new CountryRowModel(o))
                                          .ToPagedList();

            return View(bindings);
        }

        public ActionResult Create()
        {
            var model = new CountryEditorModel();

            return View(model);
        }

        public ActionResult Edit(int id)
        {
            var country = _countryService.Find(id);
            var model = new CountryEditorModel(country);

            return View(model);
        }

        [HttpPost, HandleAjaxFormError, Transactional]
        public ActionResult Save(CountryEditorModel model, string @return)
        {
            var country = new Country();
            model.UpdateTo(country);

            if (country.Id > 0)
            {
                _countryService.Update(country);
            }
            else
            {
                _countryService.Create(country);
            }

            return AjaxForm().RedirectTo(@return);
        }

        [HttpPost, HandleAjaxFormError, Transactional]
        public ActionResult Delete(CountryRowModel[] model)
        {
            foreach (var item in model)
            {
                var obj = _countryService.Find(item.Id);
                _countryService.Delete(obj);
            }

            return AjaxForm().ReloadPage();
        }
    }
}