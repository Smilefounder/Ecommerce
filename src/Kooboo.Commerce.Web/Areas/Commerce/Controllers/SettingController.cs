using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Kooboo.CMS.Common;
using Kooboo.CMS.Common.Runtime.Dependency;
using Kooboo.Commerce.Web.Areas.Commerce.Models.Settings;
using Kooboo.Commerce.Settings;
using Kooboo.Commerce.Settings.Services;
using Kooboo.Commerce.Products.Services;
using Kooboo.Commerce.Products;
using Kooboo.Commerce.Web.Framework.Mvc;

namespace Kooboo.Commerce.Web.Areas.Commerce.Controllers
{
    public class SettingController : CommerceController
    {
        private readonly ISettingService _settings;
        private readonly ICustomFieldService _customFieldService;

        public SettingController(
            ISettingService settings,
            ICustomFieldService customFieldService)
        {
            _settings = settings;
            _customFieldService = customFieldService;
        }

        public ActionResult Index()
        {
            var model = new SettingEditorModel();

            var storeSettings = _settings.Get<StoreSettings>(StoreSettings.Key) ?? new StoreSettings();
            model.StoreSetting = new StoreSettingEditorModel(storeSettings);
            model.ImageSettings = _settings.Get<ImageSettings>(ImageSettings.Key) ?? new ImageSettings();
            model.ProductSetting = new ProductSettingEditorModel(_customFieldService.PredefinedFields());

            return View(model);
        }

        [HttpPost, HandleAjaxFormError, Transactional]
        public ActionResult Index(SettingEditorModel model)
        {
            var storeSettings = new StoreSettings();
            if (model.StoreSetting != null)
            {
                model.StoreSetting.UpdateTo(storeSettings);
            }

            _settings.Set(StoreSettings.Key, storeSettings);
            _settings.Set(ImageSettings.Key, model.ImageSettings);

            if (model.ProductSetting != null)
            {
                // Update system fields
                var systemFields = new List<CustomField>();
                foreach (var fieldModel in model.ProductSetting.PredefinedFields)
                {
                    fieldModel.IsPredefined = true;

                    CustomField field = null;

                    if (fieldModel.Id > 0)
                    {
                        field = _customFieldService.GetById(fieldModel.Id);
                    }
                    else
                    {
                        field = new CustomField
                        {
                            IsPredefined = true
                        };
                    }

                    fieldModel.UpdateTo(field);

                    if (field.Id == 0)
                    {
                        _customFieldService.Create(field);
                    }
                }
            }

            return AjaxForm().ReloadPage();
        }
    }
}
