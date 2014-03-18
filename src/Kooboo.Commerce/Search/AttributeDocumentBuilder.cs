using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

namespace Kooboo.Commerce.Search
{
    public class AttributeDocumentBuilder : IDocumentBuilder
    {
        private Dictionary<string, Field[]> typeFields;

        public AttributeDocumentBuilder()
        {
            typeFields = new Dictionary<string, Field[]>();
        }

        private FieldType GetFieldType(Type propType)
        {
            if (propType.Equals(typeof(DateTime)) || propType.Equals(typeof(DateTime?)))
                return FieldType.DateTime;
            if (propType.Equals(typeof(double)) || propType.Equals(typeof(double?)))
                return FieldType.Double;
            if (propType.Equals(typeof(float)) || propType.Equals(typeof(float?)))
                return FieldType.Float;
            if (propType.Equals(typeof(int)) || propType.Equals(typeof(int?)))
                return FieldType.Int;
            if (propType.Equals(typeof(long)) || propType.Equals(typeof(long?)))
                return FieldType.Long;
            return FieldType.String;
        }

        public Field[] GetFields(string typeName)
        {
            if (typeFields.ContainsKey(typeName))
                return typeFields[typeName];

            if (!AttributedTypes.SearchTypes.ContainsKey(typeName))
                return null;

            var attr = AttributedTypes.SearchTypes[typeName];
            Type objType = attr.ObjectType;
            var props = objType.GetPropertiesWithAttribute(typeof(FieldAttribute));
            var pfs = props.Select(o => new
            {
                Property = o,
                FieldAttribute = o.GetCustomAttribute<FieldAttribute>()
            });

            var fields = pfs.OrderBy(o => o.FieldAttribute.Order)
                .Select(pf => new Field
                {
                    FieldName = string.IsNullOrEmpty(pf.FieldAttribute.FieldName) ? pf.Property.Name : pf.FieldAttribute.FieldName,
                    IsKey = pf.FieldAttribute.IsKey,
                    StoreMode = pf.FieldAttribute.StoreMode,
                    IndexMode = pf.FieldAttribute.IndexMode,
                    Boost = pf.FieldAttribute.Boost,
                    Analyzer = pf.FieldAttribute.Analyzer,
                    FieldType = GetFieldType(pf.Property.PropertyType)
                }).ToArray();

            lock (typeFields)
            {
                typeFields.Add(typeName, fields);
            }
            return fields;
        }

        public string GetAnalyzer(string typeName)
        {
            if (!AttributedTypes.SearchTypes.ContainsKey(typeName))
                return string.Empty;

            var attr = AttributedTypes.SearchTypes[typeName];
            return attr.Analyzer;
        }

        public KeyValuePair<string, string> GetKey(string typeName, IDictionary<string, object> values)
        {
            var fields = GetFields(typeName);
            var keyField = fields.First(o => o.IsKey);
            var kvp = values.FirstOrDefault(o => o.Key == keyField.FieldName);
            return new KeyValuePair<string, string>(kvp.Key, (kvp.Value ?? "").ToString());
        }
    }
}
