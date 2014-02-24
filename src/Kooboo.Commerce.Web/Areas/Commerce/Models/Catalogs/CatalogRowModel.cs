using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Kooboo.Commerce.Catalogs;
using Kooboo.Commerce.Web.Grid2;
using Kooboo.Web.Mvc.Grid2.Design;

namespace Kooboo.Commerce.Web.Areas.Commerce.Models.Catalogs {

    [Grid(Checkable = true, IdProperty = "Id")]
    public class CatalogRowModel {

        public CatalogRowModel() {
        }

        public CatalogRowModel(Catalog catalog) {
            this.Id = catalog.Id;
            this.Name = catalog.Name;
            this.Description = catalog.Description;
        }

        public int Id {
            get;
            set;
        }

        [GridColumn(GridItemColumnType = typeof(EditGridActionItemColumn))]
        public string Name {
            get;
            set;
        }

        [GridColumn]
        public string Description {
            get;
            set;
        }
    }
}