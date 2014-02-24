using Kooboo.Commerce.Data;
using Kooboo.Commerce.Web.Areas.Commerce.Models.CommerceInstances;
using Kooboo.Commerce.Web.Mvc;
using Kooboo.Commerce.Web.Mvc.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace Kooboo.Commerce.Web.Areas.Commerce.Controllers
{
    public class CommerceInstanceController : CommerceControllerBase
    {
        private ICommerceInstanceMetadataStore _metadataStore;
        private ICommerceInstanceManager _instanceManager;
        private ICommerceDbProviderFactory _dbProviderFactory;

        public CommerceInstanceController(
            ICommerceInstanceMetadataStore metadataStore,
            ICommerceInstanceManager instanceManager,
            ICommerceDbProviderFactory dbProviderFactory)
        {
            _metadataStore = metadataStore;
            _instanceManager = instanceManager;
            _dbProviderFactory = dbProviderFactory;
        }

        public ActionResult Index()
        {
            var metadatas = _instanceManager.GetAllInstanceMetadatas()
                                            .OrderBy(x => x.DisplayName)
                                            .ToList();

            var models = new List<CommerceInstanceModel>();

            foreach (var metadata in metadatas)
            {
                var model = new CommerceInstanceModel
                {
                    Name = metadata.Name,
                    DisplayName = metadata.DisplayName
                };

                var dbProvider = _dbProviderFactory.GetDbProvider(metadata.DbProviderInvariantName, metadata.DbProviderManifestToken);
                model.DbProvider = dbProvider.DisplayName;

                models.Add(model);
            }

            return View(models);
        }

        public ActionResult Create()
        {
            var model = new CommerceInstanceEditorModel();

            foreach (var provider in _dbProviderFactory.Providers)
            {
                model.AddDbProvider(provider);
            }

            return View(model);
        }

        [HttpPost, HandleAjaxFormError]
        public ActionResult Create(CommerceInstanceEditorModel model, string @return)
        {
            var metadata = new CommerceInstanceMetadata
            {
                Name = model.Name,
                DisplayName = model.DisplayName,
                DbSchema = model.DbSchema
            };

            var dbProviderKeyParts = model.DbProviderKey.Split('|');

            metadata.DbProviderInvariantName = dbProviderKeyParts[0];
            metadata.DbProviderManifestToken = dbProviderKeyParts[1];

            if (model.AdvancedMode)
            {
                metadata.ConnectionString = model.ConnectionString;
            }
            else
            {
                foreach (var param in model.ConnectionStringParameters)
                {
                    metadata.ConnectionStringParameters.Add(param.Text, param.Value);
                }
            }

            _instanceManager.CreateInstance(metadata);

            return AjaxForm().RedirectTo(@return);
        }

        public ActionResult Edit(string name)
        {
            var metadata = _instanceManager.GetInstanceMetadata(name);
            var dbProvider = _dbProviderFactory.GetDbProvider(metadata.DbProviderInvariantName, metadata.DbProviderManifestToken);

            var model = new CommerceInstanceEditorModel
            {
                IsNew = false,
                Name = name,
                DisplayName = metadata.DisplayName,
                DbSchema = metadata.DbSchema,
                DbProviderDisplayName = dbProvider.DisplayName,
                DbProviderKey = dbProvider.InvariantName + "|" + dbProvider.ManifestToken,
                ConnectionString = metadata.ConnectionString,
                ConnectionStringParameters = metadata.ConnectionStringParameters.Select(x => new SelectListItem
                {
                    Text = x.Key,
                    Value = x.Value
                })
                .ToList()
            };

            model.AdvancedMode = !String.IsNullOrEmpty(model.ConnectionString);

            foreach (var provider in _dbProviderFactory.Providers)
            {
                model.AddDbProvider(provider);
            }

            return View(model);
        }

        [HttpPost, HandleAjaxFormError]
        public ActionResult Edit(CommerceInstanceEditorModel model, string @return)
        {
            var metadata = _metadataStore.GetByName(model.Name);
            metadata.DisplayName = model.DisplayName;

            if (model.AdvancedMode)
            {
                metadata.ConnectionString = model.ConnectionString;
            }
            else
            {
                metadata.ConnectionString = null;
                metadata.ConnectionStringParameters.Clear();

                foreach (var param in model.ConnectionStringParameters)
                {
                    metadata.ConnectionStringParameters.Add(param.Text, param.Value);
                }
            }

            _metadataStore.Update(model.Name, metadata);

            return AjaxForm().RedirectTo(@return);
        }

        [HttpPost, HandleAjaxFormError]
        public ActionResult Delete(CommerceInstanceModel[] model)
        {
            _instanceManager.DeleteInstance(model[0].Name);
            return AjaxForm().ReloadPage();
        }

        public ActionResult Start(string commerceName)
        {
            return View();
        }
    }
}
