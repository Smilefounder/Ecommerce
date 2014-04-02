using Kooboo.Commerce.Settings.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Kooboo.Commerce.Products.Accessories
{
    public class ProductAccessoryService
    {
        private ISettingService _settings;

        public ProductAccessoryService(ISettingService settings)
        {
            _settings = settings;
        }

        public IEnumerable<ProductAccessory> GetAccessories(int productId)
        {
            var accessories = _settings.Get<IList<ProductAccessory>>(GetSettingEntryKey(productId)) ?? new List<ProductAccessory>();
            return accessories.OrderByDescending(x => x.Rank).ToList();
        }

        public void SaveAccessories(int productId, IEnumerable<ProductAccessory> accessories)
        {
            _settings.Set(GetSettingEntryKey(productId), accessories);
        }

        private string GetSettingEntryKey(int productId)
        {
            return "Accessories-" + productId;
        }
    }
}