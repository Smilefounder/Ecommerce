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
        Payment GetById(int id);

        IQueryable<Payment> Query();

        void Create(Payment payment);

        void HandlePaymentResult(Payment payment, ProcessPaymentResult result);
    }

    [Dependency(typeof(IPaymentService))]
    public class PaymentService : IPaymentService
    {
        private IRepository<Payment> _payments;

        public PaymentService(IRepository<Payment> payments)
        {
            _payments = payments;
        }

        public Payment GetById(int id)
        {
            return _payments.Get(id);
        }

        public IQueryable<Payment> Query()
        {
            return _payments.Query();
        }

        public void Create(Payment payment)
        {
            payment.Create();
            _payments.Insert(payment);
        }

        public void HandlePaymentResult(Payment payment, ProcessPaymentResult result)
        {
            if (payment.Status != PaymentStatus.Success)
            {
                payment.ChangeStatus(result.PaymentStatus);
            }
        }
    }
}
