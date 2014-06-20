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
        public object Config { get; private set; }

        public bool IsAsyncExecution { get; private set; }

        public ActivityContext(object config, bool isAsyncExecution)
        {
            Config = config;
            IsAsyncExecution = isAsyncExecution;
        }
    }
}
