using Kooboo.Commerce.Reflection;
using Kooboo.Commerce.Utils;
using Lucene.Net.Documents;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;

namespace Kooboo.Commerce.Search
{
    static class DocumentBuilder
    {
        public static Document Build(object model)
        {
            var type = TypeHelper.GetType(model);
            var doc = new Document();

            foreach (var prop in type.GetProperties(BindingFlags.Public | BindingFlags.Instance))
            {
                foreach (var field in BuildFields(model, prop))
                {
                    doc.Add(field);
                }
            }

            return doc;
        }

        static IEnumerable<IFieldable> BuildFields(object container, PropertyInfo property)
        {
            var fieldAttr = property.GetCustomAttribute<FieldAttribute>(false) ?? new FieldAttribute(Field.Index.NOT_ANALYZED, Field.Store.YES);
            var propType = ModelTypeInfo.GetTypeInfo(property.PropertyType);

            if (propType.IsSimpleType)
            {
                var propValue = property.GetValue(container, null);
                yield return fieldAttr.CreateLuceneField(property.Name, propValue);
            }
            else
            {
                if (propType.IsCollection)
                {
                    if (propType.IsDictionary)
                    {
                        if (!TypeHelper.IsSimpleType(propType.DictionaryKeyType))
                            throw new NotSupportedException("Not support complex dictionary key.");

                        if (TypeHelper.IsSimpleType(propType.DictionaryValueType))
                        {
                            var dic = property.GetValue(container, null) as IDictionary;
                            foreach (DictionaryEntry entry in dic)
                            {
                                yield return fieldAttr.CreateLuceneField(entry.Key.ToString(), entry.Value);
                            }
                        }
                        else
                        {
                            var valueTypeInfo = ModelTypeInfo.GetTypeInfo(propType.DictionaryValueType);
                            if (valueTypeInfo.IsCollection && !valueTypeInfo.IsDictionary && TypeHelper.IsSimpleType(valueTypeInfo.ElementType))
                            {
                                var dic = property.GetValue(container, null) as IDictionary;
                                foreach (DictionaryEntry entry in dic)
                                {
                                    var values = entry.Value as IEnumerable;
                                    foreach (var value in values)
                                    {
                                        yield return fieldAttr.CreateLuceneField(entry.Key.ToString(), value);
                                    }
                                }
                            }
                            else
                            {
                                throw new NotSupportedException("Not support dictionary property with value type other than simple type and simple type collection.");
                            }
                        }
                    }
                    else
                    {
                        var elementTypeInfo = ModelTypeInfo.GetTypeInfo(propType.ElementType);
                        if (!elementTypeInfo.IsSimpleType)
                            throw new NotSupportedException("Not support complex collection element.");

                        var items = property.GetValue(container, null) as IEnumerable;
                        if (items != null)
                        {
                            foreach (var item in items)
                            {
                                yield return fieldAttr.CreateLuceneField(property.Name, item);
                            }
                        }
                    }
                }
            }
        }
    }
}