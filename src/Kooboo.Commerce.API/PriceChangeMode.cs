using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.Api
{
    /// <summary>
    /// price change mode
    /// </summary>
    public enum PriceChangeMode
    {
        /// <summary>
        /// change the price by amount
        /// </summary>
        ByAmount = 0,
        /// <summary>
        /// change the price by the percent of itself
        /// </summary>
        ByPercent = 1
    }
}
