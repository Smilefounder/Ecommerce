using Kooboo.CMS.Common.Runtime.Dependency;
using Kooboo.Commerce.Data;
using Kooboo.Commerce.Orders;
using Kooboo.Commerce.Orders.Pricing;
using Kooboo.Commerce.Rules;
using Kooboo.Commerce.ShoppingCarts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.Promotions
{
    public class PromotionMatcher
    {
        private RuleEngine _ruleEngine;

        public PromotionMatcher(RuleEngine ruleEngine)
        {
            _ruleEngine = ruleEngine;
        }

        public IEnumerable<PromotionMatch> MatchApplicablePromotions(PricingContext context, IEnumerable<Promotion> candidatePromotions)
        {
            var matches = new List<PromotionMatch>();

            // Lower priority promotions check first
            candidatePromotions = candidatePromotions.OrderBy(x => x.Priority).ThenBy(x => x.Id);

            foreach (var promotion in candidatePromotions)
            {
                var match = TryMatchPromotion(promotion, context);
                if (match != null)
                {
                    matches.Add(match);
                }
            }

            if (matches.Count > 0)
            {
                // Higher priority check first
                matches.Reverse();
                CheckOverlappingUsage(matches);
            }

            return matches;
        }

        private void CheckOverlappingUsage(List<PromotionMatch> matches)
        {
            var matchesToExclude = new List<PromotionMatch>();
            for (var i = 0; i < matches.Count; i++)
            {
                matchesToExclude.AddRange(GetMatchesToExclude(matches, i));
            }

            foreach (var match in matchesToExclude)
            {
                matches.Remove(match);
            }
        }

        private PromotionMatch TryMatchPromotion(Promotion promotion, PricingContext context)
        {
            if (promotion.RequireCouponCode && context.CouponCode != promotion.CouponCode)
            {
                return null;
            }

            var isMatch = false;
            var conditionMatchedItems = new List<PricingItem>();

            if (!promotion.Conditions.Any())
            {
                isMatch = true;
            }
            else
            {
                var conditionChecker = new PromotionConditionChecker(_ruleEngine);
                var result = conditionChecker.CheckConditions(promotion, context);
                if (result.Success)
                {
                    isMatch = true;
                    foreach (var item in result.MatchedItems)
                    {
                        if (!conditionMatchedItems.Contains(item))
                        {
                            conditionMatchedItems.Add(item);
                        }
                    }
                }
            }

            if (isMatch)
            {
                return new PromotionMatch(promotion, conditionMatchedItems);
            }

            return null;
        }

        private List<PromotionMatch> GetMatchesToExclude(List<PromotionMatch> matches, int currentMatchIndex)
        {
            var exclusiveMatches = new List<PromotionMatch>();
            var currentPromotion = matches[currentMatchIndex].Promotion;

            for (var i = currentMatchIndex + 1; i < matches.Count; i++)
            {
                var otherMatch = matches[i];
                if (!otherMatch.Promotion.CanBeOverlappedUsedWith(currentPromotion))
                {
                    exclusiveMatches.Add(otherMatch);
                }
            }

            return exclusiveMatches;
        }
    }
}
