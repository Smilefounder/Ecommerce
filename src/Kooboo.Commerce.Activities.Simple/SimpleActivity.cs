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

        public override IEnumerable<ActivityParameter> Parameters
        {
            get
            {
                yield return new ActivityParameter("Parameter1")
                {
                    Description = "Description for parameter 1"
                };

                yield return new ActivityParameter("Parameter2")
                {
                    Description = "Description for parameter 2",
                    DefaultValue = 2048
                };
            }
        }

        protected override void DoExecute(IOrderEvent @event, ActivityContext context)
        {
            var param1 = context.ParameterValues.Get("Parameter1");
            var param2 = context.ParameterValues.Get<int>("Parameter2");

            var orderId = @event.OrderId;

            Debug.WriteLine("[" + DateTime.Now + "] SimpleActivity: Execute for order #" + orderId + ", param1: " + param1 + ", param2: " + param2);
        }
    }
}
