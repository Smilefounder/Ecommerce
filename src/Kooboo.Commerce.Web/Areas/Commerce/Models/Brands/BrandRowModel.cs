using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Kooboo.Commerce.Brands;
using Kooboo.Web.Mvc.Grid2.Design;
using Kooboo.Commerce.Web.Framework.UI.Grid2;

namespace Kooboo.Commerce.Web.Areas.Commerce.Models.Brands {

    [Grid(Checkable = true, IdProperty = "Id")]
    public class BrandRowModel {

        public BrandRowModel() {
        }

        public BrandRowModel(Brand brand) {
            this.Id = brand.Id;
            this.Name = brand.Name;
            this.Description = brand.Description;
            this.Logo = brand.Logo;
        }

        public int Id {
            get;
            set;
        }

        [LinkColumn("Edit")]
        public string Name {
            get;
            set;
        }

        [GridColumn]
        public string Description {
            get;
            set;
        }

        [GridColumn]
        public string Logo {
            get;
            set;
        }
    }
}