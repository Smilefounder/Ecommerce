using Kooboo.CMS.Common.Runtime.Dependency;
using Kooboo.Commerce.Data;
using Kooboo.Commerce.Events;
using Kooboo.Commerce.Events.Promotions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.Promotions
{
    [Dependency(typeof(PromotionService))]
    public class PromotionService
    {
        private CommerceInstance _instance;
        private IRepository<Promotion> _repository;

        public PromotionService(CommerceInstance instance)
        {
            _instance = instance;
            _repository = instance.Database.Repository<Promotion>();
        }

        public Promotion Find(int id)
        {
            return _repository.Find(id);
        }

        public IQueryable<Promotion> Query()
        {
            return _repository.Query();
        }

        public void Create(Promotion promotion)
        {
            if (promotion.RequireCouponCode && IsCouponAlreadyTaken(promotion.CouponCode, promotion.Id))
            {
                throw new InvalidOperationException("Coupon code has been taken.");
            }

            _repository.Insert(promotion);
        }

        public bool Enable(Promotion promotion)
        {
            if (promotion.IsEnabled)
            {
                return false;
            }

            promotion.IsEnabled = true;

            _repository.Database.SaveChanges();

            Event.Raise(new PromotionEnabled(promotion), _instance);

            return true;
        }

        public bool Disable(Promotion promotion)
        {
            if (!promotion.IsEnabled)
            {
                return false;
            }

            promotion.IsEnabled = false;

            _repository.Database.SaveChanges();

            Event.Raise(new PromotionDisabled(promotion), _instance);

            return true;
        }

        public void Delete(Promotion promotion)
        {
            if (promotion.IsEnabled)
            {
                Disable(promotion);
            }

            promotion.OverlappablePromotions.Clear();

            var referencingPromotions = Query().Where(p => p.OverlappablePromotions.Any(x => x.Id == promotion.Id)).ToList();

            foreach (var each in referencingPromotions)
            {
                each.RemoveOverlappablePromotion(promotion.Id);
            }

            _repository.Delete(promotion);
        }

        private bool IsCouponAlreadyTaken(string coupon, int candidatePromotionId)
        {
            return Query().Any(x => x.RequireCouponCode && x.CouponCode == coupon && x.Id != candidatePromotionId);
        }
    }
}
