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
    static class ModelConverter
    {
        public static object ParseFieldValue(Type modelType, string field, string fieldValue)
        {
            if (String.IsNullOrWhiteSpace(fieldValue) || fieldValue == "*" || fieldValue == "NULL")
            {
                return null;
            }

            var prop = modelType.GetProperty(field, BindingFlags.Public | BindingFlags.Instance);
            if (prop == null)
            {
                // if not corresponding property was found, it might be custom fields, they are all treated as string
                return fieldValue;
            }

            if (TypeHelper.IsSimpleType(prop.PropertyType))
            {
                return IndexUtil.FromFieldStringValue(fieldValue, prop.PropertyType);
            }

            var propTypeInfo = ModelTypeInfo.GetTypeInfo(prop.PropertyType);
            if (propTypeInfo.IsDictionary)
            {
                return IndexUtil.FromFieldStringValue(fieldValue, propTypeInfo.DictionaryValueType);
            }
            else if (propTypeInfo.IsCollection)
            {
                return IndexUtil.FromFieldStringValue(fieldValue, propTypeInfo.ElementType);
            }

            return null;
        }

        public static Document ToDocument(object model)
        {
            var type = TypeHelper.GetType(model);
            var doc = new Document();

            foreach (var prop in type.GetProperties(BindingFlags.Public | BindingFlags.Instance))
            {
                foreach (var field in ToFields(model, prop))
                {
                    doc.Add(field);
                }
            }

            return doc;
        }

        static IEnumerable<IFieldable> ToFields(object container, PropertyInfo property)
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
                        if (!TypeHelper.IsSimpleType(propType.ElementType))
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

        public static object ToModel(Document document, Type modelType)
        {
            var model = Activator.CreateInstance(modelType);

            foreach (var prop in modelType.GetProperties(BindingFlags.Public | BindingFlags.Instance))
            {
                var fields = document.GetFields(prop.Name);
                if (fields.Length == 0)
                {
                    continue;
                }

                if (TypeHelper.IsSimpleType(prop.PropertyType))
                {
                    var propValue = IndexUtil.FromFieldStringValue(fields[0].StringValue, prop.PropertyType);
                    prop.SetValue(model, propValue, null);
                }
                else
                {
                    var propTypeInfo = ModelTypeInfo.GetTypeInfo(prop.PropertyType);
                    if (propTypeInfo.IsCollection)
                    {
                        if (propTypeInfo.IsDictionary)
                        {
                            var propValue = prop.GetValue(model, null);
                            if (propValue == null)
                            {
                                propValue = Activator.CreateInstance(typeof(Dictionary<,>).MakeGenericType(propTypeInfo.DictionaryKeyType, propTypeInfo.DictionaryValueType));
                                prop.SetValue(model, propValue, null);
                            }

                            var dic = propValue as IDictionary;

                            foreach (var field in fields)
                            {
                                var fieldValue = IndexUtil.FromFieldStringValue(field.StringValue, propTypeInfo.DictionaryValueType);
                                dic.Add(field.Name, fieldValue);
                            }
                        }
                        else
                        {
                            var propValue = prop.GetValue(model, null);
                            var list = typeof(List<>).MakeGenericType(propTypeInfo.ElementType) as IList;

                            foreach (var field in fields)
                            {
                                var fieldValue = IndexUtil.FromFieldStringValue(field.StringValue, propTypeInfo.ElementType);
                                list.Add(fieldValue);
                            }

                            if (prop.PropertyType.IsArray)
                            {
                                prop.SetValue(model, list.OfType<object>().ToArray(), null);
                            }
                            else
                            {
                                prop.SetValue(model, list, null);
                            }
                        }
                    }
                }
            }

            return model;
        }
    }
}