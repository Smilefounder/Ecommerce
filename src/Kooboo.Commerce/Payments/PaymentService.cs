using Kooboo.CMS.Common.Runtime.Dependency;
using Kooboo.Commerce.Data;
using Kooboo.Commerce.Events;
using Kooboo.Commerce.Events.Orders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.Payments
{
    [Dependency(typeof(PaymentService))]
    public class PaymentService
    {
        private IRepository<Payment> _repository;

        public PaymentService(ICommerceDatabase database)
        {
            _repository = database.Repository<Payment>();
        }

        public Payment GetById(int id)
        {
            return _repository.Find(id);
        }

        public IQueryable<Payment> Query()
        {
            return _repository.Query();
        }

        public void Create(Payment payment)
        {
            _repository.Insert(payment);
        }

        public void AcceptProcessResult(Payment payment, ProcessPaymentResult result)
        {
            if (result.PaymentStatus == PaymentStatus.Success)
            {
                ChangeStatus(payment, PaymentStatus.Success);
            }
        }

        public void ChangeStatus(Payment payment, PaymentStatus newStatus)
        {
            if (payment.Status != newStatus)
            {
                var oldStatus = payment.Status;
                payment.Status = newStatus;

                _repository.Database.SaveChanges();

                Event.Raise(new PaymentStatusChanged(payment, oldStatus, newStatus));
            }
        }
    }
}
