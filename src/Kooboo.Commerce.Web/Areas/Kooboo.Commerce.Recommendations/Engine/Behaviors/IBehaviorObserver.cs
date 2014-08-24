﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Kooboo.Commerce.Recommendations.Engine.Behaviors
{
    public interface IBehaviorObserver
    {
        void OnReceive(IEnumerable<Behavior> behaviors);
    }
}