using Kooboo.CMS.Common.Runtime.Dependency;
using Kooboo.Commerce.Data;
using Kooboo.Commerce.Pricing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.Promotions
{
    public interface IPromotionMatcher
    {
        IEnumerable<PromotionMatch> MatchApplicablePromotions(PricingContext context);
    }

    [Dependency(typeof(IPromotionMatcher))]
    public class PromotionMatcher : IPromotionMatcher
    {
        private IRepository<Promotion> _repository;
        private IPromotionConditionFactory _conditionFactory;

        public PromotionMatcher(
            IRepository<Promotion> repository,
            IPromotionConditionFactory conditionFactory)
        {
            Require.NotNull(repository, "repository");
            Require.NotNull(conditionFactory, "conditionFactory");

            _repository = repository;
            _conditionFactory = conditionFactory;
        }

        public IEnumerable<PromotionMatch> MatchApplicablePromotions(PricingContext context)
        {
            Require.NotNull(context, "context");

            var matches = new List<PromotionMatch>();

            // Lower priority promotions check first
            var candidates = _repository.Query()
                                        .Where(x => x.IsEnabled)
                                        .OrderBy(x => x.Priority)
                                        .ThenBy(x => x.Id);


            foreach (var promotion in candidates)
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
            var isMatch = false;
            var conditionMatchedItems = new List<PricingItem>();

            if (promotion.Conditions.Count == 0)
            {
                isMatch = true;
            }
            else
            {
                // A promotion is passed when all conditions are fulfilled
                var allConditionsFulfilled = true;

                foreach (var each in promotion.Conditions)
                {
                    var condition = _conditionFactory.FindByName(each.ConditionName);
                    var result = condition.Check(new ConditionCheckingContext(promotion, each, context));

                    if (!result.Success)
                    {
                        allConditionsFulfilled = false;
                        break;
                    }
                    else
                    {
                        // Add condition matched items
                        foreach (var item in result.MatchedItems)
                        {
                            if (!conditionMatchedItems.Contains(item))
                            {
                                conditionMatchedItems.Add(item);
                            }
                        }
                    }
                }

                isMatch = allConditionsFulfilled;
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
