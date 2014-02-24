using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.Data
{
    public class EntityEventArgs<T> : System.EventArgs where T : class
    {
        public T OriginalEntity { get; set; }
        public T Entity { get; set; }
        public DataOperation Action { get; set; }
        public IRepository<T> Repository { get; set; }
    }

    public class EntitiesEventArgs<T> : System.EventArgs where T : class
    {
        public IEnumerable<T> Entities { get; set; }
        public int Count { get; set; }
        public DataOperation Action { get; set; }
        public IRepository<T> Repository { get; set; }
    }

    public delegate void EntityEventHandler<TEntity>(EntityEventArgs<TEntity> args) where TEntity : class;
    public delegate void BatchEntityEventHandler<TEntity>(EntitiesEventArgs<TEntity> args) where TEntity : class;
}
