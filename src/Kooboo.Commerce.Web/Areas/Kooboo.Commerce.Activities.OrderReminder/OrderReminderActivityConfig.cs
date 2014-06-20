using Kooboo.Commerce.Rules;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Kooboo.Commerce.Activities.OrderReminder
{
    public class OrderReminderActivityConfig : ActivityParameters
    {
        public string Subject
        {
            get
            {
                return GetValue("Subject");
            }
            set
            {
                SetValue("Subject", value);
            }
        }

        public string Body
        {
            get
            {
                return GetValue("Body");
            }
            set
            {
                SetValue("Body", value);
            }
        }

        public string Receivers
        {
            get
            {
                return GetValue("Receivers");
            }
            set
            {
                SetValue("Receivers", value);
            }
        }

        public List<Condition> CancelConditions
        {
            get
            {
                var json = GetValue("CancelConditions");
                if (!String.IsNullOrWhiteSpace(json))
                {
                    return JsonConvert.DeserializeObject<List<Condition>>(json);
                }

                return new List<Condition>();
            }
            set
            {
                if (value == null)
                {
                    SetValue("CancelConditions", null);
                }
                else
                {
                    SetValue("CancelConditions", JsonConvert.SerializeObject(value));
                }
            }
        }

        public OrderReminderActivityConfig()
        {
            CancelConditions = new List<Condition>();
        }
    }
}