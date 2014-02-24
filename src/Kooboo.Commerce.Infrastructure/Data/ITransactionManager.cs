using Kooboo.Commerce.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.Data
{
    public interface ITransactionManager : IDisposable
    {
        event EventHandler Committed;

        // TODO: 重命名为ITransactionManager，添加BeginTransaction，返回对象中包含Commit, Rollback?
        //       还是直接这样使用EF的隐式事务呢? Commit -> dbContext.SaveChanges

        void BeginTransaction();
        void Commit(bool disposeTransaction = true);
        void Rollback();

        bool IsInTransaction { get; }
        bool TransactionCommited { get; }
    }
}
