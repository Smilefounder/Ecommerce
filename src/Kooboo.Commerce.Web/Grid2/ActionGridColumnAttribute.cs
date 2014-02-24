using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Kooboo.CMS.Web.Grid2;
using Kooboo.Web.Mvc.Grid2.Design;

namespace Kooboo.Commerce.Web.Grid2 {

    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class ActionGridColumnAttribute : GridColumnAttribute {

        public ActionGridColumnAttribute() {
            this.GridItemColumnType = typeof(ActionGridColumn);
        }

        public string ActionName {
            get;
            set;
        }

        public string ButtonText {
            get;
            set;
        }

        public string ImageSrc {
            get;
            set;
        }
    }
}