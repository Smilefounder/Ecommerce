using Kooboo.CMS.Common.Runtime.Dependency;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Kooboo.Commerce.Web.Form
{
    [Dependency(typeof(IFormControl), Key = "CheckBox")]
    public class CheckBox : InputControl
    {
        public override string Name
        {
            get
            {
                return "CheckBox";
            }
        }

        public override string ValueBindingName
        {
            get
            {
                return "checked";
            }
        }

        protected override string Type
        {
            get
            {
                return "checkbox";
            }
        }
    }
}