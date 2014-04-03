using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Kooboo.CMS.Common;
using Kooboo.CMS.Common.Runtime.Dependency;
using Kooboo.Commerce.Web.Mvc.Controllers;
using Kooboo.Commerce.Web.Areas.Commerce.Models.Settings;
using Kooboo.Commerce.Settings;
using Kooboo.Commerce.Settings.Services;
using Kooboo.Commerce.Web.Mvc;
using Kooboo.Commerce.EAV.Services;
using Kooboo.Commerce.EAV;
using Kooboo.Commerce.ImageSizes.Services;

namespace Kooboo.Commerce.Web.Areas.Commerce.Controllers
{
    public class SettingController : CommerceControllerBase
    {
        private readonly ISettingService _settings;
        private readonly ICustomFieldService _customFieldService;
        private readonly IImageSizeService _imageSizeService;

        public SettingController(
            ISettingService settings,
            ICustomFieldService customFieldService,
            IImageSizeService imageSizeService)
        {
            _settings = settings;
            _customFieldService = customFieldService;
            _imageSizeService = imageSizeService;
        }

        public ActionResult Index()
        {
            var model = new SettingEditorModel();

            var storeSettings = _settings.Get<StoreSettings>(StoreSettings.Key) ?? new StoreSettings();
            model.StoreSetting = new StoreSettingEditorModel(storeSettings);

            var imageSettings = new ImageSettingsHelper(_imageSizeService).GetImageSetting();
            model.ImageSetting = new ImageSettingEditorModel(imageSettings);

            model.ProductSetting = new ProductSettingEditorModel(_customFieldService.GetSystemFields());

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

            var imageSettings = new ImageSetting();
            if (model.ImageSetting != null)
            {
                model.ImageSetting.UpdateTo(imageSettings);
            }

            new ImageSettingsHelper(_imageSizeService).SetImageSetting(imageSettings);

            if (model.ProductSetting != null)
            {
                // Update system fields
                var systemFields = new List<CustomField>();
                foreach (var fieldModel in model.ProductSetting.SystemFields)
                {
                    var field = new CustomField();
                    fieldModel.UpdateTo(field);
                    systemFields.Add(field);
                }

                _customFieldService.SetSystemFields(systemFields);
            }

            return AjaxForm().ReloadPage();
        }
    }
}
