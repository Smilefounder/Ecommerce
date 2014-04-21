using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.API.HAL
{
    public interface IContextEnviromentProvider
    {
        string Name { get; set; }
        string Description { get; set; }

        bool IsContextRunInEnviroment(HalContext context);
    }
}
