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
            this.CustomFields = new List<BrandCustomFieldModel>();
            if(brand.CustomFields != null && brand.CustomFields.Count > 0)
            {
               foreach(var cf in brand.CustomFields)
               {
                   var cfm = new BrandCustomFieldModel();
                   cfm.Name = cf.Name;
                   cfm.Value = cf.Value;
                   cfm.Text = cf.Text;

                   this.CustomFields.Add(cfm);
               }
            }
        }

        public void UpdateTo(Brand brand) {
            brand.Name = (this.Name ?? string.Empty).Trim();
            brand.Description = (this.Description ?? string.Empty).Trim();
            brand.Logo = (this.Logo ?? string.Empty).Trim();

            if(this.CustomFields != null && this.CustomFields.Count > 0)
            {
                brand.CustomFields = new List<BrandCustomField>();
                foreach (var cfm in this.CustomFields)
                {
                    var cf = new BrandCustomField();
                    cf.BrandId = this.Id;
                    cf.Name = cfm.Name;
                    cf.Value = cfm.Value;
                    cf.Text = cfm.Text;

                    brand.CustomFields.Add(cf);
                }
            }
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

        public ICollection<BrandCustomFieldModel> CustomFields { get; set; }
    }
}