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
        public ActivityRule Rule { get; private set; }

        public AttachedActivityInfo AttachedActivityInfo { get; private set; }

        public ParameterValueDictionary ParameterValues
        {
            get
            {
                return AttachedActivityInfo.ParameterValues;
            }
        }

        public bool IsAsyncExecution { get; private set; }

        public ActivityContext(ActivityRule rule, AttachedActivityInfo attachedActivityInfo, bool isAsyncExecution)
        {
            Rule = rule;
            AttachedActivityInfo = attachedActivityInfo;
            IsAsyncExecution = isAsyncExecution;
        }
    }
}
