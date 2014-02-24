using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.AddIns.Migration
{
    public class UpgradeActionDescriptor
    {
        public Type ActionType { get; private set; }

        public UpgradeActionAttribute Attribute { get; private set; }

        public UpgradeActionDescriptor(Type actionType)
        {
            ActionType = actionType;
            Attribute = actionType.GetCustomAttributes(typeof(UpgradeActionAttribute), false).FirstOrDefault() as UpgradeActionAttribute;

            if (Attribute == null)
                throw new InvalidOperationException(typeof(IUpgradeAction).FullName + " implementions must be attributed using " + typeof(UpgradeActionAttribute) + ".");
        }
    }
}
