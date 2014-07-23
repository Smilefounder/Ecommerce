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
        {
            return GetKeyNames(dbContext, typeof(TEntity));
        }

        public static string[] GetKeyNames(this DbContext dbContext, Type entityType)
        {
            //retrieve the base type
            while (entityType.BaseType != typeof(object))
            {
                entityType = entityType.BaseType;
            }

            var objectContext = ((IObjectContextAdapter)dbContext).ObjectContext;

            //create method CreateObjectSet with the generic parameter of the base-type
            var method = typeof(ObjectContext).GetMethod("CreateObjectSet", Type.EmptyTypes)
                                              .MakeGenericMethod(entityType);
            dynamic objectSet = method.Invoke(objectContext, null);

            IEnumerable<dynamic> keyMembers = objectSet.EntitySet.ElementType.KeyMembers;

            return keyMembers.Select(k => (string)k.Name).ToArray();
        }

        public static object[] GetKeys(this DbContext dbContext, object entity)
        {
            var type = entity.GetType();
            var keyNames = GetKeyNames(dbContext, type);

            object[] keys = new object[keyNames.Length];
            for (int i = 0; i < keyNames.Length; i++)
            {
                keys[i] = type.GetProperty(keyNames[i]).GetValue(entity, null);
            }

            return keys;
        }
    }
}
