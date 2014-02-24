using Kooboo.Commerce.Events;
using Kooboo.Commerce.Events.Dispatching;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading;

namespace Kooboo.Commerce.Data
{
    public interface ICommerceDbTransaction : IDisposable
    {
        bool IsCommitted { get; }

        void Commit();

        void Rollback();
    }
}
