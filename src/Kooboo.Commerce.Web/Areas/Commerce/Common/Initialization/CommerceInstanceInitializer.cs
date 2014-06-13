using Kooboo.CMS.Common.Runtime;
using Kooboo.CMS.Common.Runtime.Dependency;
using Kooboo.Commerce.Data;
using Kooboo.Commerce.Data.Initialization;
using Kooboo.Commerce.ImageSizes;
using Kooboo.Commerce.Settings;
using Kooboo.Commerce.Settings.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Kooboo.Commerce.Web.Areas.Commerce.Common.Initialization
{
    [Dependency(typeof(ICommerceInstanceInitializer), Key = "CommerceInstanceInitializer")]
    public class CommerceInstanceInitializer : ICommerceInstanceInitializer
    {
        public void Initialize(CommerceInstance instance)
        {
            InitDefaultImageSettings(instance);
        }

        private void InitDefaultImageSettings(CommerceInstance instance)
        {
            var settings = new ImageSettings
            {
                Sizes =
                {
                    CreateImageSize("List", 240, 240), 
                    CreateImageSize("Detail", 300, 300),
                    CreateImageSize("Thumbnail", 240, 240),
                    CreateImageSize("Cart", 50, 50)
                }
            };

            // TODO: 因为几乎所有 IXXXServicer 的构造函数中都依赖了 IRepository<T>，
            //       而 Repository 必须在当前上下文中存在 Commerce 实例时才可以得到，
            //       而这段代码是在刚创建完 Commerce 实例时执行的，因此无法将 ISettingService 从本类构造器中注入，
            //       因此要先创建一个上下文，然后手工调用 EngineContext.Resolve<ISettingService>，
            //       但是这样就对单元测试非常不友好了。Ideas?
            using (var scope = Scope.Begin(instance))
            {
                var service = EngineContext.Current.Resolve<ISettingService>();
                service.Set(ImageSettings.Key, settings);
            }
        }

        private ImageSize CreateImageSize(string name, int width, int height)
        {
            return new ImageSize
            {
                Name = name,
                Width = width,
                Height = height,
                IsEnabled = true
            };
        }
    }
}