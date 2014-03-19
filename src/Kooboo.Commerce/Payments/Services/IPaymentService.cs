using Kooboo.CMS.Common.Runtime.Dependency;
using Kooboo.Commerce.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.Payments.Services
{
    public interface IPaymentService
    {
        IQueryable<Payment> Query();

        void Create(Payment payment);
    }

    [Dependency(typeof(IPaymentService))]
    public class PaymentService : IPaymentService
    {
        private IRepository<Payment> _payments;

        public IQueryable<Payment> Query()
        {
            throw new NotImplementedException();
        }

        public void Create(Payment payment)
        {
            throw new NotImplementedException();
        }
    }
}
