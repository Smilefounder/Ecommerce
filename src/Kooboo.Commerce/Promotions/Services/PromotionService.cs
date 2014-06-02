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

        public bool Create(Promotion promotion)
        {
            if (promotion.RequireCouponCode && IsCouponAlreadyTaken(promotion.CouponCode, promotion.Id))
            {
                throw new InvalidOperationException("Coupon code has been taken.");
            }

            return _db.GetRepository<Promotion>().Insert(promotion);
        }

        public bool Update(Promotion promotion)
        {
            if (promotion.RequireCouponCode && IsCouponAlreadyTaken(promotion.CouponCode, promotion.Id))
            {
                throw new InvalidOperationException("Coupon code has been taken.");
            }

            return _db.GetRepository<Promotion>().Update(promotion, k => new object[] { k.Id });
        }

        public void Enable(Promotion promotion)
        {
            if (promotion.MarkEnable())
            {
                _db.SaveChanges();
                Event.Raise(new PromotionEnabled(promotion));
            }
        }

        public void Disable(Promotion promotion)
        {
            if (promotion.MarkDisable())
            {
                _db.SaveChanges();
                Event.Raise(new PromotionDisabled(promotion));
            }
        }

        public bool Delete(int promotionId)
        {
            var promotion = _db.GetRepository<Promotion>().Get(promotionId);
            if (promotion == null)
            {
                return false;
            }

            _db.GetRepository<Promotion>().Delete(promotion);

            return true;
        }

        private bool IsCouponAlreadyTaken(string coupon, int candidatePromotionId)
        {
            return Query().Any(x => x.RequireCouponCode && x.CouponCode == coupon && x.Id != candidatePromotionId);
        }
    }
}
