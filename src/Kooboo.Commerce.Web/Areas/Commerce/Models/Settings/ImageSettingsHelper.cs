using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Web.Script.Serialization;
using Kooboo.CMS.Common.Runtime.Dependency;
using Kooboo.Commerce.EAV.Services;
using Kooboo.Commerce.ImageSizes;
using Kooboo.Commerce.ImageSizes.Services;

namespace Kooboo.Commerce.Web.Areas.Commerce.Models.Settings
{

    public class ImageSettingsHelper
    {
        private IImageSizeService _service;

        public ImageSettingsHelper(IImageSizeService service)
        {
            _service = service;
        }

        public void SetImageSetting(ImageSetting setting)
        {
            //KeyValueService.Set(DefindedKeys.ImageSetting, Serialize(setting));
            // clear
            var sizes = _service.Query().ToList();
            foreach (var o in sizes) { _service.Delete(o); }
            // add
            if (setting.Thumbnail != null)
            {
                _service.Create(setting.Thumbnail);
            }
            if (setting.Detail != null)
            {
                _service.Create(setting.Detail);
            }
            if (setting.List != null)
            {
                _service.Create(setting.List);
            }
            if (setting.Cart != null)
            {
                _service.Create(setting.Cart);
            }
            if (setting.CustomSizes != null)
            {
                foreach (var size in setting.CustomSizes)
                {
                    _service.Create(size);
                }
            }
        }

        public ImageSetting GetImageSetting()
        {
            ImageSetting setting = null;
            //setting = Deserialize<ImageSetting>(KeyValueService.Get(DefindedKeys.ImageSetting));
            var sizes = _service.Query().ToList();
            if (sizes.Count > 0)
            {
                setting = new ImageSetting();
                setting.Thumbnail = sizes.Where(o => o.Name == "Thumbnail").FirstOrDefault();
                setting.Detail = sizes.Where(o => o.Name == "Detail").FirstOrDefault();
                setting.List = sizes.Where(o => o.Name == "List").FirstOrDefault();
                setting.Cart = sizes.Where(o => o.Name == "Cart").FirstOrDefault();
                setting.CustomSizes = sizes.Where(o => o.Name != "Thumbnail" && o.Name != "Detail" && o.Name != "List" && o.Name != "Cart").ToList();
            }
            return setting ?? ImageSetting.NewDefault();
        }
    }
}
