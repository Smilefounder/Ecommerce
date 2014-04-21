using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Kooboo.Commerce.API.HAL;

namespace Kooboo.Commerce.API.LocalProvider
{
    public interface IHalContextAware
    {
        /// <summary>
        /// hal context
        /// </summary>
        HalContext HalContext { get; set; }
    }
}
