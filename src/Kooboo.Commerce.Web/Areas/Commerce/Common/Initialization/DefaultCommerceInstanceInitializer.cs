using Kooboo.CMS.Common.Runtime;
using Kooboo.CMS.Common.Runtime.Dependency;
using Kooboo.Commerce.Data;
using Kooboo.Commerce.Data.Initialization;
using Kooboo.Commerce.Settings;
using Kooboo.Commerce.Settings.Services;
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

            // TODO: 因为几乎所有 IXXXServicer 的构造函数中都依赖了 IRepository<T>，
            //       而 Repository 必须在当前上下文中存在 Commerce 实例时才可以得到，
            //       而这段代码是在刚创建完 Commerce 实例时执行的，因此无法将 ISettingService 从本类构造器中注入，
            //       因此要先创建一个上下文，然后手工调用 EngineContext.Resolve<ISettingService>，
            //       但是这样就对单元测试非常不友好了。Ideas?
            using (var scope = Scope.Begin(instance))
            {
                var service = EngineContext.Current.Resolve<ISettingService>();
                service.Set(settings);
            }
        }
    }
}