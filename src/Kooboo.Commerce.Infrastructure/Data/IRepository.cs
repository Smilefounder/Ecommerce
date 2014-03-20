using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Kooboo.Commerce.Data
{
    public interface IRepository
    {
        ICommerceDatabase Database { get; }

        Type EntityType { get; }

        object Get(object id);

        IQueryable Query();

        void Insert(object entity);

        void Delete(object entity);
    }
}