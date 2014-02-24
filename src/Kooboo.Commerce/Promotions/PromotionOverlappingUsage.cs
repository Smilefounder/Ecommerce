using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.Promotions
{
    public enum PromotionOverlappingUsage
    {
        [Description("Not Allowed")]
        NotAllowed = 0,
        
        [Description("Allowed with any promotion")]
        AllowedWithAnyPromotion = 1,

        [Description("Allowed with specified promotions")]
        AllowedWithSpecifiedPromotions = 2
    }
}
