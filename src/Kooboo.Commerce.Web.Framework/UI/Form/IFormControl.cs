using Kooboo.Commerce.EAV;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Kooboo.Commerce.Web.Framework.UI.Form
{
    public interface IFormControl
    {
        string Name { get; }

        string ValueBindingName { get; }

        IHtmlString Render(CustomField field, string value, object htmlAttributes, ViewContext viewContext);
    }
}