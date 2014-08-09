using Kooboo.Commerce.Reflection;
using Kooboo.Commerce.Utils;
using Lucene.Net.Documents;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Web;

namespace Kooboo.Commerce.Search
{
    /// <summary>
    /// Converts poco model to lucene document, or converts lucene document to poco model.
    /// </summary>
    /// <remarks>
    /// A poco model is converted to document with these rules:
    /// 
    /// 1. For simple property (e.g., Int32, String, Decimal)
    ///    - Create a normal lucene field (or numeric field)
    ///    
    /// 2. For list/set/array property
    ///    - Create a multi-valued field
    ///    
    /// 3. For dictionary property
    ///    - Create a field for each key, and the field name format is PropertyName[DictionaryKey]
    ///    - The dictionary value can be a simple type or a list/set, if it's a list/set, a multi-valued field is created for each key
    /// </remarks>
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

        static readonly Regex _dictionaryFieldNameRegex = new Regex(@"^(?<prop>\w+)\[(?<key>[\w\-]+)\]$", RegexOptions.Compiled | RegexOptions.IgnoreCase);

        public static string GetDictionaryFieldName(string propertyName, string dictionaryKey)
        {
            return propertyName + "[" + dictionaryKey + "]";
        }

        public static bool TryParseDictionaryFieldName(string fieldName, out string propertyName, out string dictionaryKey)
        {
            var match = _dictionaryFieldNameRegex.Match(fieldName);
            if (match.Success)
            {
                propertyName = match.Groups["prop"].Value;
                dictionaryKey = match.Groups["key"].Value;
                return true;
            }
            else
            {
                propertyName = null;
                dictionaryKey = null;
                return false;
            }
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
                                var fieldName = GetDictionaryFieldName(property.Name, entry.Key.ToString());
                                yield return fieldAttr.CreateLuceneField(fieldName, entry.Value);
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
                                    var fieldName = GetDictionaryFieldName(property.Name, entry.Key.ToString());
                                    var values = entry.Value as IEnumerable;
                                    foreach (var value in values)
                                    {
                                        yield return fieldAttr.CreateLuceneField(fieldName, value);
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
                if (TypeHelper.IsSimpleType(prop.PropertyType))
                {
                    var field = document.GetField(prop.Name);
                    if (field != null)
                    {
                        var propValue = IndexUtil.FromFieldStringValue(field.StringValue, prop.PropertyType);
                        prop.SetValue(model, propValue, null);
                    }
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
                            var fields = document.GetFields().Where(f => f.Name.StartsWith(prop.Name));

                            // Property type is IDictionary<TKey, TValue>
                            if (TypeHelper.IsSimpleType(propTypeInfo.DictionaryValueType))
                            {
                                foreach (var field in fields)
                                {
                                    string propName;
                                    string dicKey;

                                    if (TryParseDictionaryFieldName(field.Name, out propName, out dicKey))
                                    {
                                        var fieldValue = IndexUtil.FromFieldStringValue(field.StringValue, propTypeInfo.DictionaryValueType);
                                        dic.Add(dicKey, fieldValue);
                                    }
                                }
                            }
                            else // Property type is IDictionary<TKey, IList<TValue>> or IDictionary<TKey, ISet<TValue>>
                            {
                                var dicValueTypeInfo = ModelTypeInfo.GetTypeInfo(propTypeInfo.DictionaryValueType);
                                if (dicValueTypeInfo.IsCollection && TypeHelper.IsSimpleType(dicValueTypeInfo.ElementType))
                                {
                                    Type newDicValueType = null;
                                    MethodInfo hashsetAddMethod = null;
                                    if (dicValueTypeInfo.IsSet)
                                    {
                                        newDicValueType = typeof(HashSet<>).MakeGenericType(dicValueTypeInfo.ElementType);
                                        hashsetAddMethod = GetAddMethod(newDicValueType, dicValueTypeInfo.ElementType);
                                    }
                                    else
                                    {
                                        newDicValueType = typeof(List<>).MakeGenericType(dicValueTypeInfo.ElementType);
                                    }

                                    foreach (var field in fields)
                                    {
                                        string propName;
                                        string dicKey;

                                        if (TryParseDictionaryFieldName(field.Name, out propName, out dicKey))
                                        {
                                            var fieldValue = IndexUtil.FromFieldStringValue(field.StringValue, dicValueTypeInfo.ElementType);
                                            if (!dic.Contains(dicKey))
                                            {
                                                dic.Add(dicKey, Activator.CreateInstance(newDicValueType));
                                            }

                                            var list = dic[dicKey];

                                            if (dicValueTypeInfo.IsSet) // is HashSet<>
                                            {
                                                hashsetAddMethod.Invoke(list, new[] { fieldValue });
                                            }
                                            else // is IList<>
                                            {
                                                (list as IList).Add(fieldValue);
                                            }
                                        }
                                    }
                                }
                            }
                        }
                        else // Property is collection but not dictionary
                        {
                            var fields = document.GetFields(prop.Name);
                            if (fields.Length == 0)
                            {
                                continue;
                            }

                            var list = Activator.CreateInstance(typeof(List<>).MakeGenericType(propTypeInfo.ElementType)) as IList;

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

        static MethodInfo GetAddMethod(Type collectionType, Type elementType)
        {
            return collectionType.GetMethods().First(m =>
            {
                if (!m.IsPublic || m.Name != "Add")
                {
                    return false;
                }

                var parameters = m.GetParameters();
                if (parameters.Length != 1)
                {
                    return false;
                }
                if (parameters[0].ParameterType != elementType)
                {
                    return false;
                }

                return true;
            });
        }
    }
}