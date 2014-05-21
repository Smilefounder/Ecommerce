using Kooboo.Commerce.Events;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.Activities
{
    /// <summary>
    /// 定义可挂到事件上执行的Activity。
    /// </summary>
    public interface IActivity
    {
        /// <summary>
        /// Activity的惟一名称。
        /// </summary>
        string Name { get; }

        /// <summary>
        /// 是否允许异步执行。
        /// </summary>
        bool AllowAsyncExecution { get; }

        /// <summary>
        /// 该Activity是否可以绑定到指定的事件。
        /// </summary>
        bool CanBindTo(Type eventType);

        /// <summary>
        /// 执行Activity。
        /// </summary>
        /// <remarks>
        /// 若Activity执行过程中发现业务逻辑不满足要求，
        /// 应抛出 <see cref="Kooboo.Commerce.BusinessRuleViolationException"/>，
        /// UI应特殊处理该类异常。其余异常将被认为是服务器端异常。
        /// </remarks>
        /// <param name="event">当前触发的事件。</param>
        /// <param name="context">其它上下文信息。</param>
        void Execute(IEvent @event, ActivityContext context);

        /// <summary>
        /// 获取Activity的配置编辑器。
        /// </summary>
        ActivityEditor GetEditor();
    }
}
