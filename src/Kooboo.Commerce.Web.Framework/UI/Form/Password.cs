using Kooboo.CMS.Common.Runtime.Dependency;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Kooboo.Commerce.Web.Framework.UI.Form
{
    [Dependency(typeof(IFormControl), Key = "Password")]
    public class Password : InputControl
    {
        protected override string Type
        {
            get
            {
                return "password";
            }
        }

        public override bool IsValuesPredefined
        {
            get { return false; }
        }

        public override bool IsSelectionList
        {
            get { return false; }
        }

        public override string Name
        {
            get
            {
                return "Password";
            }
        }
    }
}