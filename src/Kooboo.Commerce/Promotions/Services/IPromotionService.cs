using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.Promotions.Services
{
    public interface IPromotionService
    {
        Promotion GetById(int id);

        IQueryable<Promotion> Query();

        void Enable(Promotion promotion);

        void Disable(Promotion promotion);

        bool Create(Promotion promotion);

        bool Delete(Promotion promotion);
    }
}
