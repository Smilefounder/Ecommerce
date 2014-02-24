using Kooboo.CMS.Common.Runtime.Dependency;
using Kooboo.Commerce.Events.Dispatching;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.Data
{
    // TODO: 考虑更换文件位置? Infrastructure初衷是用于基础设施，和Commerce无关，但这些接口则是带有Commerce信息的

    // TODO: 这里的CommerceDatabase是虚拟的Database，一个database只是一组指定了schema的表，和SQL Server的Database不是同一概念。
    //       为了防止误解，是否要换一个名字，不以Database结尾？
    public interface ICommerceDatabase : IDisposable
    {
        CommerceInstanceMetadata Metadata { get; }

        IRepository<T> GetRepository<T>() where T : class;

        ICommerceDbTransaction BeginTransaction();

        ICommerceDbTransaction BeginTransaction(IsolationLevel isolationLevel);

        void SaveChanges();
    }
}
