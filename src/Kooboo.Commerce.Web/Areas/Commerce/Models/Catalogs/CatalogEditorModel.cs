using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Kooboo.Commerce.Catalogs;

namespace Kooboo.Commerce.Web.Areas.Commerce.Models.Catalogs {

    public class CatalogEditorModel {

        public CatalogEditorModel() {
        }

        public CatalogEditorModel(Catalog catalog) {
            this.Id = catalog.Id;
            this.Name = catalog.Name;
            this.Description = catalog.Description;
        }

        public void UpdateTo(Catalog catalog) {
            catalog.Name = (this.Name ?? string.Empty).Trim();
            catalog.Description = (this.Description ?? string.Empty).Trim();
        }

        public int Id {
            get;
            set;
        }

        [Required(ErrorMessage = "Required")]
        [AdditionalMetadata("class", "medium")]
        public string Name {
            get;
            set;
        }

        public string Description {
            get;
            set;
        }
    }
}