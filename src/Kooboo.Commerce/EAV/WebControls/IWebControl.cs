using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace Kooboo.Commerce.EAV.WebControls
{
    public interface IWebControl
    {
        string Name { get; }

        IHtmlString Render(WebControlModel model, ViewContext viewContext);
    }
}
