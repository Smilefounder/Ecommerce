using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using Kooboo.Web.Mvc;
using Kooboo.Commerce.Settings;
using Kooboo.Commerce.Web.Areas.Commerce.Models.DataSources;

namespace Kooboo.Commerce.Web.Areas.Commerce.Models.Settings {

    public class StoreSettingEditorModel {

        public StoreSettingEditorModel() {
        }

        public StoreSettingEditorModel(StoreSetting shopSetting) {
            this.PriceIndex = shopSetting.PriceIndex;
            this.Culture = shopSetting.Culture;
            this.CurrencyISOCode = shopSetting.CurrencyISOCode;
            this.WeightUnitName = shopSetting.WeightUnitName;
            this.SizeUnitName = shopSetting.SizeUnitName;
        }

        public void UpdateTo(StoreSetting shopSetting) {
            shopSetting.PriceIndex = this.PriceIndex;
            shopSetting.Culture = this.Culture;
            shopSetting.CurrencyISOCode = this.CurrencyISOCode;
            shopSetting.WeightUnitName = this.WeightUnitName;
            shopSetting.SizeUnitName = this.SizeUnitName;
        }

        [Required]
        [Description("Raise all prices by the specified percentage.")]
        public decimal PriceIndex {
            get;
            set;
        }

        [Required]
        [UIHint("DropDownList")]
        [DataSource(typeof(CultureDataSource))]
        [Description("The culture shown on your website. This is used to display currency, date, number and other culture related content.")]
        public string Culture {
            get;
            set;
        }

        [Required]
        [UIHint("DropDownList")]
        [DataSource(typeof(CurrencyDataSource))]
        [Description("Select a currency for the prices of your products.")]
        public string CurrencyISOCode {
            get;
            set;
        }

        [Required]
        public string WeightUnitName {
            get;
            set;
        }

        [Required]
        public string SizeUnitName {
            get;
            set;
        }
    }
}