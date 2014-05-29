using Kooboo.CMS.Common.Runtime;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Kooboo.Commerce.Web.Form
{
    public static class FormControls
    {
        public static IEnumerable<IFormControl> Controls()
        {
            return EngineContext.Current.ResolveAll<IFormControl>();
        }
    }
}