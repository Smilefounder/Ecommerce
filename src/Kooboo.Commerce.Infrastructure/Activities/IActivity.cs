using Kooboo.Commerce.Events;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.Activities
{
    public interface IActivity
    {
        ActivityResult Execute(IEvent evnt, ActivityContext context);
    }
}
