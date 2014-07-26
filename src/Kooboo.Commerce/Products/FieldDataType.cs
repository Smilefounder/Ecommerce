#region License
// 
// Copyright (c) 2013, Kooboo team
// 
// Licensed under the BSD License
// See the file LICENSE.txt for details.
// 
#endregion
using System;

namespace Kooboo.Commerce.EAV
{
    /// <summary>
    /// Use in cms.
    /// </summary>
    public enum FieldDataType
    {
        String = 0,
        Int = 1,
        Decimal = 2,
        DateTime = 3,
        Bool = 4
    }
    /// <summary>
    /// 
    /// </summary>
    public static class FieldDataTypeHelper
    {
        #region Methods
        /// <summary>
        /// Defaults the value.
        /// </summary>
        /// <param name="dataType">Type of the data.</param>
        /// <returns></returns>
        public static object DefaultValue(FieldDataType dataType)
        {
            switch (dataType)
            {
                case FieldDataType.String:
                    return "";
                case FieldDataType.Int:
                    return default(int);
                case FieldDataType.Decimal:
                    return default(decimal);
                case FieldDataType.DateTime:
                    return DateTime.UtcNow;
                case FieldDataType.Bool:
                    return default(bool);
                default:
                    return null;
            }
        }

        /// <summary>
        /// Parses the value.
        /// </summary>
        /// <param name="dataType">Type of the data.</param>
        /// <param name="value">The value.</param>
        /// <param name="throwWhenInvalid">if set to <c>true</c> [throw when invalid].</param>
        /// <returns></returns>
        /// <exception cref="Kooboo.KoobooException"></exception>
        public static object ParseValue(FieldDataType dataType, string value, bool throwWhenInvalid)
        {
            switch (dataType)
            {
                case FieldDataType.String:
                    return value;
                case FieldDataType.Int:
                    int intValue;
                    if (int.TryParse(value, out intValue))
                    {
                        return intValue;
                    }
                    else
                    {
                        if (throwWhenInvalid)
                        {
                            throw new InvalidCastException("The value is invalid.");
                        }
                        return default(int);
                    }
                case FieldDataType.Decimal:
                    decimal decValue;
                    if (decimal.TryParse(value, out decValue))
                    {
                        return decValue;
                    }
                    else
                    {
                        if (throwWhenInvalid)
                        {
                            throw new InvalidCastException("The value is invalid.");
                        }
                        return default(decimal);
                    }
                case FieldDataType.DateTime:
                    DateTime dateTime;
                    if (DateTime.TryParse(value, out dateTime))
                    {
                        if (dateTime.Kind != DateTimeKind.Utc)
                        {
                            dateTime = new DateTime(dateTime.Ticks, DateTimeKind.Local).ToUniversalTime();
                        }
                        return dateTime;

                    }
                    else
                    {
                        if (throwWhenInvalid)
                        {
                            throw new InvalidCastException("The value is invalid.");
                        }
                        return default(DateTime);
                    }
                case FieldDataType.Bool:
                    if (!string.IsNullOrEmpty(value))
                    {
                        bool boolValue;
                        if (bool.TryParse(value, out boolValue))
                        {
                            return boolValue;
                        }
                        else
                        {
                            if (throwWhenInvalid)
                            {
                                throw new InvalidCastException("The value is invalid.");
                            }
                            return default(bool);
                        }
                    }
                    else
                    {
                        return false;
                    }
                default:
                    return null;
            }

        }
        #endregion
    }
}
