using Lucene.Net.Documents;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace Kooboo.Commerce.Search
{
    static class IndexUtil
    {
        public static string ToFieldStringValue(object value)
        {
            if (value == null)
            {
                return "<NULL>";
            }

            if (value is DateTime || value is DateTime?)
            {
                return DateTools.DateToString((DateTime)value, DateTools.Resolution.MILLISECOND);
            }

            return value.ToString();
        }

        public static object FromFieldStringValue(string value, Type fieldType)
        {
            if (value == "<NULL>")
            {
                return null;
            }

            if (fieldType == typeof(DateTime) || fieldType == typeof(DateTime?))
            {
                return DateTools.StringToDate(value);
            }

            return Convert.ChangeType(value, fieldType);
        }
    }
}