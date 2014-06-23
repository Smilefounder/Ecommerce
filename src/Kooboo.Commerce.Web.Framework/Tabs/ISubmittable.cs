using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.Web.Framework.Tabs
{
    public interface ISubmittable
    {
        Type ModelType { get; }

        void OnSubmit(TabSubmitContext context);
    }
}
