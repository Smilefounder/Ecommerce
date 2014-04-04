using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.Shipping.Services
{
    public interface IShippingMethodService
    {
        ShippingMethod GetById(int id);

        IQueryable<ShippingMethod> Query();

        bool Create(ShippingMethod method);

        bool Delete(ShippingMethod method);

        void Enable(ShippingMethod method);

        void Disable(ShippingMethod method);
    }
}
