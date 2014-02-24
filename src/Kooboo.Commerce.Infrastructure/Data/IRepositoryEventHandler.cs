using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.Data
{
    public interface IRepositoryEventHandler<T> where T : class
    {
        bool CanHandle();

        EntityEventHandler<T> InsertingHandler { get; set; }
        EntityEventHandler<T> InsertedHandler { get; set; }
        EntityEventHandler<T> UpdatingHandler { get; set; }
        EntityEventHandler<T> UpdatedHandler { get; set; }
        EntityEventHandler<T> DeletingHandler { get; set; }
        EntityEventHandler<T> DeletedHandler { get; set; }

        BatchEntityEventHandler<T> BatchInsertingHandler { get; set; }
        BatchEntityEventHandler<T> BatchInsertedHandler { get; set; }
        BatchEntityEventHandler<T> BatchUpdatingHandler { get; set; }
        BatchEntityEventHandler<T> BatchUpdatedHandler { get; set; }
        BatchEntityEventHandler<T> BatchDeletingHandler { get; set; }
        BatchEntityEventHandler<T> BatchDeletedHandler { get; set; }
    }
}
