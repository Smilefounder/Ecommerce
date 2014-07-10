using Kooboo.Commerce.Customers;
using Kooboo.Commerce.Orders;
using Kooboo.Commerce.Products;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;

namespace Kooboo.Commerce.Promotions
{
    /// <summary>
    /// 定义一种促销策略。
    /// </summary>
    public interface IPromotionPolicy
    {
        /// <summary>
        /// 策略名称。
        /// </summary>
        string Name { get; }

        /// <summary>
        /// 存储配置信息的对象的类型。
        /// </summary>
        Type ConfigModelType { get; }

        /// <summary>
        /// 执行促销策略。
        /// </summary>
        void Execute(PromotionContext context);
    }
}
