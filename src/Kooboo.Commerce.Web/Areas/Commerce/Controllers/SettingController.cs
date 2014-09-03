using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Kooboo.CMS.Common;
using Kooboo.CMS.Common.Runtime.Dependency;
using Kooboo.Commerce.Web.Areas.Commerce.Models.Settings;
using Kooboo.Commerce.Settings;
using Kooboo.Commerce.Products;
using Kooboo.Commerce.Web.Framework.Mvc;

namespace Kooboo.Commerce.Web.Areas.Commerce.Controllers
{
    public class SettingController : CommerceController
    {
        private readonly SettingService _settings;
        private readonly PredefinedCustomFieldService _predefinedFields;

        public SettingController(SettingService settings, PredefinedCustomFieldService predefinedFields)
        {
            _settings = settings;
            _predefinedFields = predefinedFields;
        }

        public ActionResult Index()
        {
            var model = new SettingsModel();

            model.Global = _settings.Get<GlobalSettings>();
            model.PredefinedFields = _predefinedFields.Query().ToList();

            return View(model);
        }

        [HttpPost, HandleAjaxFormError, Transactional]
        public ActionResult Index(SettingsModel model)
        {
            _settings.Set(model.Global);

            if (model.PredefinedFields != null)
            {
                for (var i = 0; i < model.PredefinedFields.Count; i++)
                {
                    model.PredefinedFields[i].Sequence = i;
                }

                _predefinedFields.UpdateWith(model.PredefinedFields);
            }

            return AjaxForm().ReloadPage();
        }
    }
}
