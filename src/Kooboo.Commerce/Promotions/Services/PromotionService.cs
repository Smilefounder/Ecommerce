using Kooboo.CMS.Common.Runtime.Dependency;
using Kooboo.Commerce.Data;
using Kooboo.Commerce.Events;
using Kooboo.Commerce.Events.Promotions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.Promotions.Services
{
    [Dependency(typeof(IPromotionService))]
    public class PromotionService : IPromotionService
    {
        private ICommerceDatabase _db;

        public PromotionService(ICommerceDatabase db)
        {
            _db = db;
        }

        public Promotion GetById(int id)
        {
            return _db.GetRepository<Promotion>().Get(o => o.Id == id);
        }

        public IQueryable<Promotion> Query()
        {
            return _db.GetRepository<Promotion>().Query();
        }

        public void Create(Promotion promotion)
        {
            if (promotion.RequireCouponCode && IsCouponAlreadyTaken(promotion.CouponCode, promotion.Id))
            {
                throw new InvalidOperationException("Coupon code has been taken.");
            }

            _db.GetRepository<Promotion>().Insert(promotion);
        }

        public bool Enable(Promotion promotion)
        {
            if (promotion.IsEnabled)
            {
                return false;
            }

            promotion.IsEnabled = true;
            _db.SaveChanges();

            Event.Raise(new PromotionEnabled(promotion));

            return true;
        }

        public bool Disable(Promotion promotion)
        {
            if (!promotion.IsEnabled)
            {
                return false;
            }

            promotion.IsEnabled = false;
            _db.SaveChanges();

            Event.Raise(new PromotionDisabled(promotion));

            return true;
        }

        public void Delete(Promotion promotion)
        {
            if (promotion.IsEnabled)
            {
                Disable(promotion);
            }

            _db.GetRepository<Promotion>().Delete(promotion);
        }

        private bool IsCouponAlreadyTaken(string coupon, int candidatePromotionId)
        {
            return Query().Any(x => x.RequireCouponCode && x.CouponCode == coupon && x.Id != candidatePromotionId);
        }
    }
}
