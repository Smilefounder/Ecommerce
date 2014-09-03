using Kooboo.CMS.Common.Runtime;
using Kooboo.CMS.Common.Runtime.Dependency;
using Kooboo.Commerce.Data;
using Kooboo.Commerce.Data.Initialization;
using Kooboo.Commerce.Settings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Kooboo.Commerce.Web.Initialization
{
    public class DefaultCommerceInstanceInitializer : IInstanceInitializer
    {
        public void Initialize(CommerceInstance instance)
        {
            InitGlobalSettings(instance);
        }

        private void InitGlobalSettings(CommerceInstance instance)
        {
            var settings = new GlobalSettings
            {
                Image = new ImageSettings
                {
                    Types =
                    {
                        new ImageType("List", 240, 240), 
                        new ImageType("Detail", 300, 300, true),
                        new ImageType("Thumbnail", 240, 240),
                        new ImageType("Cart", 50, 50)
                    }
                }
            };

            var service = new SettingService(instance.Database);
            service.Set(settings);
        }
    }
}