using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Web.Script.Serialization;
using Kooboo.CMS.Common.Runtime.Dependency;
using Kooboo.Commerce.EAV.Services;
using Kooboo.Commerce.ImageSizes;
using Kooboo.Commerce.ImageSizes.Services;
using Kooboo.Commerce.Settings.Services;

namespace Kooboo.Commerce.Settings.Services {

    [Dependency(typeof(ISettingService))]
    public class SettingService : ISettingService {

        #region Services Inject

        [Inject]
        public IKeyValueService KeyValueService {
            get;
            set;
        }

        [Inject]
        public IImageSizeService ImageSizeService {
            get;
            set;
        }

        [Inject]
        public ICustomFieldService CustomFieldService {
            get;
            set;
        }

        #endregion

        #region ISettingService Members

        public void SetStoreSetting(StoreSetting setting) {
            //KeyValueService.Set(DefindedKeys.StoreSetting, Serialize(setting));
            var flags = BindingFlags.DeclaredOnly | BindingFlags.Public | BindingFlags.Instance;
            var declaredFields = typeof(StoreSetting).GetProperties(flags).Where(o => o.CanRead && o.CanWrite);
            foreach (var field in declaredFields) {
                var val = field.GetValue(setting, null);
                KeyValueService.Set(field.Name, (val != null) ? val.ToString() : null, DefindedKeys.StoreSetting);
            }
        }

        public StoreSetting GetStoreSetting() {
            StoreSetting setting = null;
            //setting = Deserialize<StoreSetting>(KeyValueService.Get(DefindedKeys.StoreSetting));
            var flags = BindingFlags.DeclaredOnly | BindingFlags.Public | BindingFlags.Instance;
            var declaredFields = typeof(StoreSetting).GetProperties(flags).Where(o => o.CanRead && o.CanWrite);
            var fieldValues = KeyValueService.GetByCategory(DefindedKeys.StoreSetting);
            foreach (var field in declaredFields) {
                var fieldVal = fieldValues.Where(o => o.Key == field.Name).FirstOrDefault();
                if (fieldVal != null) {
                    if (setting == null) {
                        setting = new StoreSetting();
                    }
                    object val = null;
                    if (field.PropertyType == typeof(String)) {
                        val = fieldVal.Value;
                    } else if (field.PropertyType == typeof(Int32)) {
                        int intVal = 0;
                        if (int.TryParse(fieldVal.Value, out intVal)) {
                            val = intVal;
                        }
                    } else if (field.PropertyType == typeof(Decimal)) {
                        decimal decimalVal = 0;
                        if (decimal.TryParse(fieldVal.Value, out decimalVal)) {
                            val = decimalVal;
                        }
                    }
                    field.SetValue(setting, val, null);
                }
            }
            return setting ?? StoreSetting.NewDefault();
        }

        public void SetImageSetting(ImageSetting setting) {
            //KeyValueService.Set(DefindedKeys.ImageSetting, Serialize(setting));
            // clear
            var sizes = ImageSizeService.Query().ToList();
            foreach (var o in sizes) { ImageSizeService.Delete(o); }
            // add
            if (setting.Thumbnail != null) {
                ImageSizeService.Create(setting.Thumbnail);
            }
            if (setting.Detail != null) {
                ImageSizeService.Create(setting.Detail);
            }
            if (setting.List != null) {
                ImageSizeService.Create(setting.List);
            }
            if (setting.Cart != null) {
                ImageSizeService.Create(setting.Cart);
            }
            if (setting.CustomSizes != null) {
                foreach (var size in setting.CustomSizes) {
                    ImageSizeService.Create(size);
                }
            }
        }

        public ImageSetting GetImageSetting() {
            ImageSetting setting = null;
            //setting = Deserialize<ImageSetting>(KeyValueService.Get(DefindedKeys.ImageSetting));
            var sizes = ImageSizeService.Query().ToList();
            if (sizes.Count > 0) {
                setting = new ImageSetting();
                setting.Thumbnail = sizes.Where(o => o.Name == "Thumbnail").FirstOrDefault();
                setting.Detail = sizes.Where(o => o.Name == "Detail").FirstOrDefault();
                setting.List = sizes.Where(o => o.Name == "List").FirstOrDefault();
                setting.Cart = sizes.Where(o => o.Name == "Cart").FirstOrDefault();
                setting.CustomSizes = sizes.Where(o => o.Name != "Thumbnail" && o.Name != "Detail" && o.Name != "List" && o.Name != "Cart").ToList();
            }
            return setting ?? ImageSetting.NewDefault();
        }

        public void SetProductSetting(ProductSetting setting) {
            //KeyValueService.Set(DefindedKeys.ProductSetting, Serialize(setting));
            CustomFieldService.SetSystemFields(setting.SystemFields);
        }

        public ProductSetting GetProductSetting() {
            ProductSetting setting = null;
            //setting = Deserialize<ProductSetting>(KeyValueService.Get(DefindedKeys.ProductSetting));
            var fields = CustomFieldService.GetSystemFields();
            if (fields.Count() > 0) {
                setting = new ProductSetting() { SystemFields = fields.ToList() };
            }
            return setting ?? ProductSetting.NewDefault();
        }

        #endregion

        #region Helpers

        public class DefindedKeys {
            public const string StoreSetting = "System_StoreSetting";
            public const string ImageSetting = "System_ImageSetting";
            public const string ProductSetting = "System_ProductSetting";
        }

        static string Serialize(object obj) {
            if (obj == null) { return null; }
            var serializer = new JavaScriptSerializer();
            return serializer.Serialize(obj);
        }

        static T Deserialize<T>(string json) {
            if (string.IsNullOrEmpty(json)) {
                return default(T);
            }
            try {
                var serializer = new JavaScriptSerializer();
                return serializer.Deserialize<T>(json);
            } catch (Exception) {
                return default(T);
            }
        }

        #endregion
    }
}
