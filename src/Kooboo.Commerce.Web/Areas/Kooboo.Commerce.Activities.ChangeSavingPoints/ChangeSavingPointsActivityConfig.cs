using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Kooboo.Commerce.Activities.ChangeSavingPoints
{
    public class ChangeSavingPointsActivityConfig : ActivityParameters
    {
        public SavingPointAction Action
        {
            get
            {
                return GetValue<SavingPointAction>("Action");
            }
            set
            {
                SetValue("Action", value);
            }
        }

        public int Amount
        {
            get
            {
                return GetValue<int>("Amount");
            }
            set
            {
                SetValue("Amount", value);
            }
        }
    }
}