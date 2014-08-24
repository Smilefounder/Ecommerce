using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Kooboo.Commerce.Recommendations.Engine
{
    static class Formulas
    {
        public static double TimeAttenuationFactor(DateTime timestamp1, DateTime timestamp2, float alpha)
        {
            return 1 / (1 + alpha * Math.Abs((timestamp1 - timestamp2).TotalSeconds));
        }
    }
}