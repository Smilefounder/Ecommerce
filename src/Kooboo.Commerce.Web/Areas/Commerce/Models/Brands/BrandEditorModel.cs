using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Kooboo.Commerce.Brands;

namespace Kooboo.Commerce.Web.Areas.Commerce.Models.Brands {

    public class BrandEditorModel {

        public BrandEditorModel() {
        }

        public BrandEditorModel(Brand brand) {
            this.Id = brand.Id;
            this.Name = brand.Name;
            this.Description = brand.Description;
            this.Logo = brand.Logo;
        }

        public void UpdateTo(Brand brand) {
            brand.Name = (this.Name ?? string.Empty).Trim();
            brand.Description = (this.Description ?? string.Empty).Trim();
            brand.Logo = (this.Logo ?? string.Empty).Trim();
        }

        public int Id {
            get;
            set;
        }

        public string Name {
            get;
            set;
        }

        public string Description {
            get;
            set;
        }

        public string Logo {
            get;
            set;
        }
    }
}