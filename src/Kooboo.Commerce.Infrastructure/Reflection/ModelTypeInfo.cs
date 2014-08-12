using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Kooboo.Commerce.Reflection
{
    public class ModelTypeInfo
    {
        public Type Type { get; private set; }

        /// <summary>
        /// If the property is a simple property like string, int, decimal, etc.
        /// </summary>
        public bool IsSimpleType { get; private set; }

        public bool IsCollection { get; private set; }

        public bool IsSet { get; private set; }

        public Type ElementType { get; private set; }

        public bool IsDictionary { get; private set; }

        public Type DictionaryKeyType { get; private set; }

        public Type DictionaryValueType { get; private set; }

        public ModelTypeInfo(Type type)
        {
            Require.NotNull(type, "type");

            Type = type;

            IsSimpleType = TypeHelper.IsSimpleType(type);

            if (!IsSimpleType)
            {
                if (type.IsArray)
                {
                    IsCollection = true;
                    ElementType = type.GetElementType();
                }
                else
                {
                    InspectCollectionInfo(type);

                    foreach (var @interface in type.GetInterfaces())
                    {
                        InspectCollectionInfo(@interface);
                    }
                }
            }
        }

        private void InspectCollectionInfo(Type type)
        {
            if (!type.IsGenericType) return;

            var genericDef = type.GetGenericTypeDefinition();
            if (genericDef == typeof(IEnumerable<>))
            {
                IsCollection = true;
                ElementType = type.GetGenericArguments()[0];
            }
            if (genericDef == typeof(ISet<>))
            {
                IsSet = true;
            }
            if (genericDef == typeof(IDictionary<,>))
            {
                IsDictionary = true;
                var genericArgs = type.GetGenericArguments();
                DictionaryKeyType = genericArgs[0];
                DictionaryValueType = genericArgs[1];
            }
        }

        static readonly ConcurrentDictionary<Type, ModelTypeInfo> _cache = new ConcurrentDictionary<Type, ModelTypeInfo>();

        public static ModelTypeInfo GetTypeInfo(Type type)
        {
            Require.NotNull(type, "type");
            return _cache.GetOrAdd(type, key => new ModelTypeInfo(key));
        }
    }
}
