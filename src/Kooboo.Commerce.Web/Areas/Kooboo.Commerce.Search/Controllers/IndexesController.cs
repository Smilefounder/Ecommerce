using Kooboo.Commerce.Web.Framework.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Kooboo.Extensions;
using System.Globalization;
using Kooboo.Commerce.Multilingual.Storage;
using Kooboo.Commerce.Products;
using Kooboo.Commerce.Search.Rebuild;
using Kooboo.Commerce.Data;
using Kooboo.Commerce.Search.Controllers.Models;
using Kooboo.Commerce.Search.Models;

namespace Kooboo.Commerce.Search.Controllers
{
    public class IndexesController : CommerceController
    {
        private ILanguageStore _languageStore;

        public IndexesController(ILanguageStore languageStore)
        {
            _languageStore = languageStore;
        }

        public ActionResult Index()
        {
            var models = new List<IndexModel>();
            models.AddRange(CreateIndexModels("Products", typeof(ProductModel)));
            return View(models);
        }

        private List<IndexModel> CreateIndexModels(string name, Type modelType)
        {
            var languages = new List<string> { String.Empty };
            languages.AddRange(_languageStore.All().Select(l => l.Name));

            var models = new List<IndexModel>();

            foreach (var lang in languages)
            {
                var model = new IndexModel
                {
                    Name = name + (!String.IsNullOrEmpty(lang) ? " (" + lang + ")" : String.Empty),
                    Culture = lang,
                    DocumentType = modelType.AssemblyQualifiedNameWithoutVersion()
                };

                var task = RebuildTasks.GetTask(CurrentInstance.Name, modelType, CultureInfo.GetCultureInfo(lang));
                UpdateIndexModel(model, task);

                models.Add(model);
            }

            return models;
        }

        private void UpdateIndexModel(IndexModel model, RebuildTask task)
        {
            model.IsRebuilding = task.Status == RebuildStatus.Running;
            model.RebuildProgress = task.Progress;

            var taskInfo = task.GetTaskInfo();
            model.LastSucceededRebuildTimeUtc = taskInfo.LastSucceededRebuildTimeUtc;
            model.LastRebuildStatus = taskInfo.LastRebuildStatus;
            model.LastRebuildError = taskInfo.LastRebuildError;
            model.LastRebuildErrorDetail = taskInfo.LastRebuildErrorDetail;
        }

        public ActionResult GetRebuildingInfo(string keys)
        {
            var results = new List<object>();
            var keyArray = keys.Split(new[] { "||" }, StringSplitOptions.RemoveEmptyEntries);
            foreach (var key in keyArray)
            {
                var parts = key.Split('|');
                var task = RebuildTasks.GetTask(CurrentInstance.Name, Type.GetType(parts[0], true), CultureInfo.GetCultureInfo(parts[1]));
                
                var result = new IndexModel();
                UpdateIndexModel(result, task);

                results.Add(result);
            }

            return JsonNet(results).UsingClientConvention();
        }

        [HttpPost]
        public void Rebuild(string documentType, string culture)
        {
            var cultureInfo = String.IsNullOrEmpty(culture) ? CultureInfo.InvariantCulture : CultureInfo.GetCultureInfo(culture);
            var task = RebuildTasks.GetTask(CurrentInstance.Name, Type.GetType(documentType, true), cultureInfo);
            task.Start();
        }

        [HttpPost, HandleAjaxError]
        public void CancelRebuild(string documentType, string culture)
        {
            var cultureInfo = String.IsNullOrEmpty(culture) ? CultureInfo.InvariantCulture : CultureInfo.GetCultureInfo(culture);
            var task = RebuildTasks.GetTask(CurrentInstance.Name, Type.GetType(documentType, true), cultureInfo);
            task.Cancel();
        }
    }
}
