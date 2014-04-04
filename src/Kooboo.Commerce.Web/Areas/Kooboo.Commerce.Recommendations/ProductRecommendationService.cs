using Kooboo.CMS.Common.Runtime.Dependency;
using Kooboo.Commerce.Settings.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Kooboo.Commerce.Recommendations
{
    [Dependency(typeof(IProductRecommendationService))]
    public class ProductRecommendationService : IProductRecommendationService
    {
        private ISettingService _settingService;

        public ProductRecommendationService(ISettingService settingService)
        {
            _settingService = settingService;
        }

        public IEnumerable<ProductRecommendation> GetRecommendations(int productId)
        {
            var key = GetSettingsEntryKey(productId);
            var recommendations = _settingService.Get<IList<ProductRecommendation>>(key);
            return recommendations ?? new List<ProductRecommendation>();
        }

        public void SaveRecommendations(int productId, IEnumerable<ProductRecommendation> recommendations)
        {
            var key = GetSettingsEntryKey(productId);
            _settingService.Set(key, recommendations);
        }

        private string GetSettingsEntryKey(int productId)
        {
            return "Recommendations-" + productId;
        }
    }
}