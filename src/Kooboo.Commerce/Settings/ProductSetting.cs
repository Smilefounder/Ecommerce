using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Kooboo.Commerce.EAV;

namespace Kooboo.Commerce.Settings {

    public class ProductSetting {

        public static ProductSetting NewDefault() {
            return new ProductSetting() {
                SystemFields = new List<CustomField>()
            };
        }

        public List<CustomField> SystemFields {
            get;
            set;
        }
    }
}
