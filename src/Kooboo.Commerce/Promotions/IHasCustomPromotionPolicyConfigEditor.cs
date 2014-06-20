using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.Promotions
{
    public interface IHasCustomPromotionPolicyConfigEditor
    {
        string GetEditorVirtualPath(Promotion promotion);
    }
}
