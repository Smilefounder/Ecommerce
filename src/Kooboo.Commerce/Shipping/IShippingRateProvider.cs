using Kooboo.Commerce.Customers;
using Kooboo.Commerce.ShoppingCarts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;

namespace Kooboo.Commerce.Shipping
{
    /// <summary>
    /// 定义运费计算接口。
    /// </summary>
    public interface IShippingRateProvider
    {
        string Name { get; }

        /// <summary>
        /// 计算运费。
        /// </summary>
        decimal GetShippingRate(ShippingMethod shippingMethod, ShippingRateCalculationContext context);

        /// <summary>
        /// 获取自定义配置的编辑器信息。
        /// </summary>
        ShippingRateProviderEditor GetEditor(ShippingMethod shippingMethod);
    }
}
