using Kooboo.Commerce.API.HAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.API.LocalProvider
{
    /// <summary>
    /// local commerce query and data access
    /// </summary>
    /// <typeparam name="T">api object type</typeparam>
    /// <typeparam name="Model">entity type</typeparam>
    public abstract class LocalCommerceQueryAccess<T, Model> : LocalCommerceQuery<T, Model>, ICommerceAccess<T>
        where T : class, IItemResource, new()
        where Model : class, new()
    {
        public LocalCommerceQueryAccess(IHalWrapper halWrapper, IMapper<T, Model> mapper)
            : base(halWrapper, mapper)
        {
        }

        /// <summary>
        /// create object
        /// </summary>
        /// <param name="obj">object</param>
        /// <returns>true if successfully, else false</returns>
        public abstract bool Create(T obj);

        /// <summary>
        /// update object
        /// </summary>
        /// <param name="obj">object</param>
        /// <returns>true if successfully, else false</returns>
        public abstract bool Update(T obj);

        /// <summary>
        /// create/update object
        /// </summary>
        /// <param name="obj">object</param>
        /// <returns>true if successfully, else false</returns>
        public abstract bool Save(T obj);

        /// <summary>
        /// delete object
        /// </summary>
        /// <param name="obj">object</param>
        /// <returns>true if successfully, else false</returns>
        public abstract bool Delete(T obj);

        protected override IQueryable<Model> CreateQuery()
        {
            throw new NotImplementedException();
        }

        protected override IQueryable<Model> OrderByDefault(IQueryable<Model> query)
        {
            throw new NotImplementedException();
        }
    }
}
