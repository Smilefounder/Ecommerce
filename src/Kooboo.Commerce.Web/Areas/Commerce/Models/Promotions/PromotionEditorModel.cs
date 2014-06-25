using Kooboo.Commerce.Promotions;
using Kooboo.Commerce.Rules;
using Kooboo.Commerce.Web.Areas.Commerce.Models.Conditions;
using Kooboo.Commerce.Web.Framework.ComponentModel.DataAnnotations;
using Kooboo.Commerce.Web.Mvc;
using Kooboo.Web.Mvc.Grid2.Design;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Kooboo.Commerce.Web.Areas.Commerce.Models.Promotions
{
    [Grid]
    public class PromotionEditorModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Required")]
        [AdditionalMetadata("class", "medium")]
        public string Name { get; set; }

        [AdditionalMetadata("class", "medium")]
        [Display(Name = "Start time")]
        public DateTime? StartTime { get; set; }

        [AdditionalMetadata("class", "medium")]
        [Display(Name = "End time")]
        public DateTime? EndTime { get; set; }

        [Display(Name = "Requires coupon code")]
        public bool RequireCouponCode { get; set; }

        [Display(Name = "Coupon code")]
        [RequiredIf("RequireCouponCode", true)]
        public string CouponCode { get; set; }

        public int Priority { get; set; }

        [Required(ErrorMessage = "Required")]
        public string PromotionPolicy { get; set; }

        [Display(Name = "Overlapping usage")]
        public PromotionOverlappingUsage OverlappingUsage { get; set; }

        public IList<SelectListItem> AvailableOverlappingUsages { get; set; }

        public IList<PromotionRowModel> OtherPromotions { get; set; }

        public PromotionEditorModel()
        {
            OtherPromotions = new List<PromotionRowModel>();
            AvailableOverlappingUsages = SelectListItems.FromEnum(typeof(PromotionOverlappingUsage));
        }

        public void UpdateFrom(Promotion promotion)
        {
            Id = promotion.Id;
            Name = promotion.Name;
            StartTime = promotion.StartTimeUtc == null ? null : (DateTime?)promotion.StartTimeUtc.Value.ToLocalTime();
            EndTime = promotion.EndTimeUtc == null ? null : (DateTime?)promotion.EndTimeUtc.Value.ToLocalTime();
            RequireCouponCode = promotion.RequireCouponCode;
            CouponCode = promotion.CouponCode;
            Priority = promotion.Priority;
            PromotionPolicy = promotion.PromotionPolicyName;
            OverlappingUsage = promotion.OverlappingUsage;
        }

        public void UpdateSimplePropertiesTo(Promotion promotion)
        {
            promotion.Name = Name.Trim();
            promotion.StartTimeUtc = StartTime == null ? null : (DateTime?)StartTime.Value.ToUniversalTime();
            promotion.EndTimeUtc = EndTime == null ? null : (DateTime?)EndTime.Value.ToUniversalTime();
            promotion.RequireCouponCode = RequireCouponCode;
            promotion.CouponCode = CouponCode.TrimOrNull();
            promotion.Priority = Priority;
            promotion.PromotionPolicyName = PromotionPolicy;
            promotion.OverlappingUsage = OverlappingUsage;
        }
    }
}