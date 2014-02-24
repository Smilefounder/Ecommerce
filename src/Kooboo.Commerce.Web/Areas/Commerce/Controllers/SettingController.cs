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

namespace Kooboo.Commerce.Web.Areas.Commerce.Controllers
{
    public class SettingController : CommerceControllerBase
    {
        private readonly ISettingService _settingService;

        public SettingController(ISettingService settingService)
        {
            _settingService = settingService;
        }

        public ActionResult Index()
        {
            var storeSetting = _settingService.GetStoreSetting();
            var imageSetting = _settingService.GetImageSetting();
            var productSetting = _settingService.GetProductSetting();
            var model = new SettingEditorModel(storeSetting, imageSetting, productSetting);
            return View(model);
        }

        [HttpPost, HandleAjaxFormError, Transactional]
        public ActionResult Index(SettingEditorModel model)
        {
            var storeSett = new StoreSetting();
            if (model.StoreSetting != null)
                model.StoreSetting.UpdateTo(storeSett);
            _settingService.SetStoreSetting(storeSett);

            var imageSett = new ImageSetting();
            if (model.ImageSetting != null)
                model.ImageSetting.UpdateTo(imageSett);
            _settingService.SetImageSetting(imageSett);

            var productSett = new ProductSetting();
            if (model.ProductSetting != null)
                model.ProductSetting.UpdateTo(productSett);
            _settingService.SetProductSetting(productSett);

            return AjaxForm().ReloadPage();
        }
    }
}
