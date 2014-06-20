using Kooboo.CMS.Common.Runtime.Dependency;
using Kooboo.Commerce.Events.Orders;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kooboo.Commerce.Activities.Simple
{
    [Dependency(typeof(IActivity), Key = "SimpleActivity")]
    public class SimpleActivity : ActivityBase<IOrderEvent>
    {
        public override string Name
        {
            get
            {
                return "SimpleActivity";
            }
        }

        public override Type ParametersType
        {
            get
            {
                return typeof(SimpleActivityParameters);
            }
        }

        protected override void DoExecute(IOrderEvent @event, ActivityContext context)
        {
            var param = context.Parameters as SimpleActivityParameters;

            var orderId = @event.OrderId;

            Debug.WriteLine("[" + DateTime.Now + "] SimpleActivity: Execute for order #" + orderId + ", param1: " + param.Parameter1 + ", param2: " + param.Parameter2);
        }
    }
}
