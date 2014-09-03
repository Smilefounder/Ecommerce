using Kooboo.CMS.Common.Runtime.Dependency;
using Kooboo.Commerce.Settings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Kooboo.Commerce.Accessories
{
    [Dependency(typeof(IProductAccessoryService))]
    public class ProductAccessoryService : IProductAccessoryService
    {
        private SettingService _settings;

        public ProductAccessoryService(SettingService settings)
        {
            _settings = settings;
        }

        public IEnumerable<ProductAccessory> GetAccessories(int productId)
        {
            var accessories = _settings.Get<IList<ProductAccessory>>(GetSettingEntryKey(productId)) ?? new List<ProductAccessory>();
            return accessories.OrderByDescending(x => x.Rank).ToList();
        }

        public void UpdateAccessories(int productId, IEnumerable<ProductAccessory> accessories)
        {
            _settings.Set(GetSettingEntryKey(productId), accessories);
        }

        private string GetSettingEntryKey(int productId)
        {
            return "Accessories-" + productId;
        }
    }
}