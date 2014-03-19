using Kooboo.CMS.Common.Runtime.Dependency;
using Kooboo.Commerce.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.Promotions.Services
{
    [Dependency(typeof(IPromotionService))]
    public class PromotionService : IPromotionService
    {
        private IRepository<Promotion> _repository;

        public PromotionService(IRepository<Promotion> repository)
        {
            _repository = repository;
        }

        public Promotion GetById(int id)
        {
            return _repository.Get(o => o.Id == id);
        }

        public IQueryable<Promotion> Query()
        {
            return _repository.Query();
        }

        public bool Create(Promotion promotion)
        {
            if (promotion.RequireCouponCode && !IsCouponAvailable(promotion.CouponCode, promotion.Id))
            {
                throw new InvalidOperationException("Coupon code has been taken.");
            }

            return _repository.Insert(promotion);
        }

        public bool Update(Promotion promotion)
        {
            if (promotion.RequireCouponCode && !IsCouponAvailable(promotion.CouponCode, promotion.Id))
            {
                throw new InvalidOperationException("Coupon code has been taken.");
            }

            return _repository.Update(promotion, k => new object[] { k.Id });
        }

        public void Enable(Promotion promotion)
        {
            promotion.Enable();
        }

        public void Disable(Promotion promotion)
        {
            promotion.Disable();
        }

        public bool Delete(Promotion promotion)
        {
            return _repository.Delete(promotion);
        }

        public bool IsCouponAvailable(string coupon, int candidatePromotionId)
        {
            return !Query().Any(x => 
                x.RequireCouponCode && x.CouponCode == coupon && x.Id != candidatePromotionId);
        }
    }
}
