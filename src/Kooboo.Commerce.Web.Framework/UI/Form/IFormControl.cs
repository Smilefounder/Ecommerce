using Kooboo.Commerce.Products;
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

        bool IsValuesPredefined { get; }

        bool IsSelectionList { get; }

        IHtmlString Render(CustomFieldDefinition fieldDefinition, string fieldValue, object htmlAttributes, ViewContext viewContext);

        string GetFieldDisplayText(CustomFieldDefinition fieldDefinition, string fieldValue);
    }
}