using Kooboo.CMS.Common.Runtime.Dependency;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Kooboo.Commerce.Web.Form
{
    [Dependency(typeof(IFormControl), Key = "CheckBoxList")]
    public class CheckBoxList : IFormControl
    {
        public string Name
        {
            get
            {
                return "CheckBoxList";
            }
        }

        public string ValueBindingName
        {
            get { throw new NotImplementedException(); }
        }

        public IHtmlString Render(EAV.CustomField field, string value, object htmlAttributes, System.Web.Mvc.ViewContext viewContext)
        {
            throw new NotImplementedException();
        }
    }
}