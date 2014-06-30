using Kooboo.CMS.Common.Runtime.Dependency;
using Kooboo.Commerce.Data;
using Kooboo.Commerce.Events;
using Kooboo.Commerce.Events.Orders;
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

        void AcceptProcessResult(Payment payment, ProcessPaymentResult result);

        void ChangeStatus(Payment payment, PaymentStatus newStatus);
    }
}
