using Kooboo.Commerce.Web.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce
{
    public static class ObjectExtensions
    {
        public static string ToJson(this object data, PropertyNaming propertyNaming)
        {
            if (data == null)
            {
                return null;
            }

            var settings = new JsonSerializerSettings();

            if (propertyNaming == PropertyNaming.CamelCase)
            {
                settings.ContractResolver = new CamelCasePropertyNamesContractResolver();
            }

            settings.Converters.Add(new StringEnumConverter());

            return JsonConvert.SerializeObject(data, settings);
        }

        public static object GetKey(this object entity)
        {
            object key;

            if (!TryGetKey(entity, out key))
                throw new InvalidOperationException("Cannot resolve key property.");

            return key;
        }

        public static bool TryGetKey(this object entity, out object key)
        {
            Require.NotNull(entity, "entity");

            key = null;

            var prop = entity.GetType().GetKeyProperty();
            if (prop != null)
            {
                key = prop.GetValue(entity, null);
                return true;
            }

            return false;
        }

        public static T ConvertTo<T>(this object obj, T defaultValule)
        {
            return (T)ConvertTo(obj, typeof(T), defaultValule);
        }

        public static object ConvertTo(this object obj, Type type, object defaultValue)
        {
            if (obj == null || obj == DBNull.Value || string.IsNullOrWhiteSpace(obj.ToString()))
                return defaultValue;
            if (obj.GetType().IsGenericType)
            {
                var definitionType = obj.GetType().GetGenericTypeDefinition();
                if (definitionType.Equals(typeof(Nullable<>)))
                {
                    var prop = type.GetProperty("Value");
                    obj = prop.GetValue(obj, null);
                }
            }
            if (type.IsGenericType)
            {
                Type[] realTypes = type.GetGenericArguments();
                if (realTypes.Length == 1)
                {
                    return ConvertTo(obj, realTypes[0], defaultValue);
                }
            }

            if (type.IsEnum)
            {
                //int val = -1;
                //if (int.TryParse(obj.ToString(), out val))
                //{
                //    return ((object)val);
                //}
                //else
                //{
                return Enum.Parse(type, obj.ToString());
                //}
            }
            else if (typeof(Guid).IsAssignableFrom(type))
            {
                Guid guid = new Guid(obj.ToString());
                return guid as object;
            }
            else if (typeof(bool).IsAssignableFrom(type))
            {
                bool val = false;
                switch (obj.ToString().ToLower())
                {
                    case "on":
                    case "yes":
                    case "1":
                    case "true":
                        val = true;
                        break;
                    default:
                        val = false;
                        break;
                }
                return val as object;
            }
            else if (typeof(byte[]).IsAssignableFrom(type))
            {
                if (type == typeof(string))
                {
                    return Encoding.UTF8.GetBytes(obj.ToString());
                }
                else if (type == typeof(char))
                {
                    return BitConverter.GetBytes((char)obj);
                }
                else if (type == typeof(int))
                {
                    return BitConverter.GetBytes((int)obj);
                }
                else if (type == typeof(float))
                {
                    return BitConverter.GetBytes((float)obj);
                }
                else if (type == typeof(double))
                {
                    return BitConverter.GetBytes((double)obj);
                }
                else if (type == typeof(decimal))
                {
                    return BitConverter.GetBytes((double)obj);
                }
                else if (type == typeof(long))
                {
                    return BitConverter.GetBytes((long)obj);
                }
                else if (type == typeof(short))
                {
                    return BitConverter.GetBytes((short)obj);
                }
                else if (type == typeof(uint))
                {
                    return BitConverter.GetBytes((uint)obj);
                }
                else if (type == typeof(ulong))
                {
                    return BitConverter.GetBytes((ulong)obj);
                }
                else if (type == typeof(ushort))
                {
                    return BitConverter.GetBytes((ushort)obj);
                }
            }
            else if (typeof(System.Xml.Linq.XElement).IsAssignableFrom(type))
            {
                return System.Xml.Linq.XElement.Parse(obj.ToString());
            }
            else
            {
                try
                {
                    return Convert.ChangeType(obj, type);
                }
                catch { }
            }

            return defaultValue;
        }

    }
}
