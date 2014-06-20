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

        Type ConfigModelType { get; }

        /// <summary>
        /// 计算运费。
        /// </summary>
        decimal GetShippingRate(ShippingRateCalculationContext context);
    }
}
