using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.Settings.Services {

    public interface ISettingService {

        void SetStoreSetting(StoreSetting setting);

        StoreSetting GetStoreSetting();


        void SetImageSetting(ImageSetting setting);

        ImageSetting GetImageSetting();


        void SetProductSetting(ProductSetting setting);

        ProductSetting GetProductSetting();
    }
}
