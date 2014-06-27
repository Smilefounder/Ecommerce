using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Core.Objects;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.Data
{
    public static class DbContextExtensions
    {
        public static string[] GetKeyNames<TEntity>(this DbContext dbContext)
            where TEntity : class
        {
            var type = typeof(TEntity);

            //retrieve the base type
            while (type.BaseType != typeof(object))
            {
                type = type.BaseType;
            }

            var objectContext = ((IObjectContextAdapter)dbContext).ObjectContext;

            //create method CreateObjectSet with the generic parameter of the base-type
            var method = typeof(ObjectContext).GetMethod("CreateObjectSet", Type.EmptyTypes)
                                              .MakeGenericMethod(type);
            dynamic objectSet = method.Invoke(objectContext, null);

            IEnumerable<dynamic> keyMembers = objectSet.EntitySet.ElementType.KeyMembers;

            return keyMembers.Select(k => (string)k.Name).ToArray();
        }

        public static object[] GetKeys<TEntity>(this DbContext dbContext, TEntity entity) 
            where TEntity : class
        {
            var keyNames = GetKeyNames<TEntity>(dbContext);
            var type = typeof(TEntity);

            object[] keys = new object[keyNames.Length];
            for (int i = 0; i < keyNames.Length; i++)
            {
                keys[i] = type.GetProperty(keyNames[i]).GetValue(entity, null);
            }

            return keys;
        }
    }
}
