using Kooboo.Commerce.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.Tests.Common.Data
{
    public class MockDbTransaction : ICommerceDbTransaction
    {
        public bool IsCommitted { get; private set; }

        public void Commit()
        {
            IsCommitted = true;
        }

        public void Rollback()
        {
        }

        public void Dispose()
        {
        }
    }
}
