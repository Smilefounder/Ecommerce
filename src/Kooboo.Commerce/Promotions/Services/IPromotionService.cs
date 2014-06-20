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

        bool Enable(Promotion promotion);

        bool Disable(Promotion promotion);

        void Create(Promotion promotion);

        void Delete(Promotion promotion);
    }
}
