using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Kooboo.CMS.Common;
using Kooboo.CMS.Common.Runtime.Dependency;
using Kooboo.Commerce.Data;
using Kooboo.Commerce.Web.Mvc.Controllers;
using Kooboo.Commerce.Web.Mvc.Paging;
using Kooboo.Commerce.Web.Areas.Commerce.Models.Catalogs;
using Kooboo.Commerce.Catalogs;

namespace Kooboo.Commerce.Web.Areas.Commerce.Controllers {

    public class CatalogController : CommerceControllerBase {

        [Inject]
        public IRepository<Catalog> CatalogRepository {
            get;
            set;
        }

        public ActionResult Index(int? page, int? pageSize) {
            var catalogs = CatalogRepository.Query()
                .OrderByDescending(x => x.Id)
                .ToPagedList(page, pageSize)
                .Transform(x => new CatalogRowModel(x));
            // ret
            return View(catalogs);
        }

        public ActionResult Create() {
            var model = new CatalogEditorModel();
            return View(model);
        }

        public ActionResult Edit(int id) {
            var catalog = CatalogRepository.Load(id);
            var model = new CatalogEditorModel(catalog);
            return View(model);
        }

        [HttpPost]
        public ActionResult Save(CatalogEditorModel model, string @return) {
            var data = new JsonResultData();

            data.RunWithTry(_ => {
                Catalog catalog = null;

                if (model.Id > 0) {
                    catalog = CatalogRepository.Load(model.Id);
                    model.UpdateTo(catalog);
                } else {
                    catalog = new Catalog();
                    model.UpdateTo(catalog);
                    CatalogRepository.Create(catalog);
                }

                UnitOfWork.Commit();
                data.RedirectUrl = @return;
            });
            // ret
            return Json(data);
        }

        [HttpPost]
        public ActionResult Delete(CatalogRowModel[] model) {
            var data = new JsonResultData(ModelState);

            data.RunWithTry(_ => {
                var ids = model.Select(x => x.Id).ToArray();
                var catalogs = CatalogRepository.Query()
                    .Where(x => ids.Contains(x.Id))
                    .ToList();

                foreach (var catalog in catalogs) {
                    CatalogRepository.Delete(catalog);
                }

                UnitOfWork.Commit();
                data.ReloadPage = true;
            });

            return Json(data);
        }
    }
}
