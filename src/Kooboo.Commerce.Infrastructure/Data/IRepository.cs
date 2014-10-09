using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace Kooboo.Commerce.Data
{
    public interface IRepository
    {
        ICommerceDatabase Database { get; }

        Type EntityType { get; }

        object Find(params object[] ids);

        IQueryable Query();

        void Create(object entity);

        void Update(object entity);

        void Delete(object entity);
    }
}
