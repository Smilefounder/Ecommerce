using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.AddIns.Migration
{
    public interface IUpgradeAction
    {
        void Do();

        void Undo();
    }
}
