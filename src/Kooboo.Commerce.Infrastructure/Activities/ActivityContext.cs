using Kooboo.Commerce.Collections;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.Activities
{
    public class ActivityContext
    {
        public ActivityParameters Parameters { get; private set; }

        public bool IsAsyncExecution { get; private set; }

        public ActivityContext(ActivityParameters parameters, bool isAsyncExecution)
        {
            Parameters = parameters;
            IsAsyncExecution = isAsyncExecution;
        }
    }
}
