using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Kooboo.CMS.Common;
using Kooboo.Commerce.Data;
using Kooboo.Commerce.Locations;
using Kooboo.Commerce.Web.Areas.Commerce.Models.Countries;
using Kooboo.Commerce.Web.Mvc.Controllers;
using Kooboo.Commerce.Locations.Services;

namespace Kooboo.Commerce.Web.Areas.Commerce.Controllers
{
    public class CountryController : CommerceControllerBase
    {
        private readonly ICountryService _countryService;

        public CountryController(ICountryService countryService)
        {
            _countryService = countryService;
        }

        public ActionResult Index(int? page, int? pageSize)
        {
            var bindings = _countryService.Query()
                .OrderBy(o => o.Name)
                .ToPagedList(page, pageSize)
                .Transform(o => new CountryRowModel(o));

            return View(bindings);
        }

        public ActionResult Create()
        {
            var model = new CountryEditorModel();

            return View(model);
        }

        public ActionResult Edit(int id)
        {
            var country = _countryService.GetById(id);
            var model = new CountryEditorModel(country);

            return View(model);
        }

        [HttpPost]
        public ActionResult Save(CountryEditorModel model, string @return)
        {
            var data = new JsonResultData();

            data.RunWithTry(_ =>
            {
                var country = new Country();
                model.UpdateTo(country);
                _countryService.Save(country);
                data.RedirectUrl = @return;
            });

            return Json(data);
        }

        [HttpPost]
        public ActionResult Delete(CountryRowModel[] model)
        {
            var data = new JsonResultData(ModelState);

            data.RunWithTry(_ =>
            {
                foreach (var item in model)
                {
                    var obj = _countryService.GetById(item.Id);
                    _countryService.Delete(obj);
                }
                data.ReloadPage = true;
            });

            return Json(data);
        }
    }
}