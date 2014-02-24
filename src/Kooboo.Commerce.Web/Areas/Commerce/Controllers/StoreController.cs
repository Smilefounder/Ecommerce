using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Kooboo.CMS.Common;
using Kooboo.Commerce.Data;
using Kooboo.Commerce.Stores;
using Kooboo.Commerce.Web.Areas.Commerce.Models.Stores;
using Kooboo.Commerce.Web.Mvc.Controllers;
using Kooboo.Commerce.Web.Mvc.Paging;

namespace Kooboo.Commerce.Web.Areas.Commerce.Controllers
{
    public class StoreController : CommerceControllerBase
    {
        private readonly IRepository<Store> _storeRepository;

        public StoreController(IRepository<Store> storeRepository)
        {
            _storeRepository = storeRepository;
        }

        public ActionResult Index(int? page, int? pageSize)
        {
            var bindings =  _storeRepository
                .Query()
                .OrderByDescending(store => store.Id)
                .ToPagedList(page, pageSize)
                .Transform(binding =>
                    {
                        return new StoreRowModel(binding);
                    });

            return View(bindings);
        }

        public ActionResult Create()
        {
            var model = new StoreEditorModel();

            return View(model);
        }

        public ActionResult Edit(int id)
        {
            var store = _storeRepository.Load(id);
            var model = new StoreEditorModel(store);

            return View(model);
        }

        [HttpPost]
        public ActionResult Save(StoreEditorModel model, string @return)
        {
            var data = new JsonResultData();

            data.RunWithTry(_ =>
            {
                Store store = null;

                if (model.Id > 0)
                {
                    store = _storeRepository.Load(model.Id);
                    model.UpdateTo(store);
                }
                else
                {
                    store = new Store();
                    model.UpdateTo(store);
                    _storeRepository.Create(store);
                }

                UnitOfWork.Commit();
                data.RedirectUrl = @return;
            });

            return Json(data);
        }

        [HttpPost]
        public ActionResult Delete(StoreRowModel[] model)
        {
            var data = new JsonResultData(ModelState);

            data.RunWithTry(_ =>
            {
                var ids = model.Select(x => x.Id).ToArray();
                var bindings = _storeRepository
                    .Query()
                    .Where(x => ids.Contains(x.Id))
                    .ToList();

                foreach (var binding in bindings)
                {
                    _storeRepository.Delete(binding);
                }

                UnitOfWork.Commit();
                data.ReloadPage = true;
            });

            return Json(data);
        }
    }
}