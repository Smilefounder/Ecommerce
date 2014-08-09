using Lucene.Net.Documents;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace Kooboo.Commerce.Search
{
    static class LuceneUtility
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

        public static int GetSortType(Type fieldType)
        {
            if (fieldType == typeof(short))
            {
                return Lucene.Net.Search.SortField.SHORT;
            }
            if (fieldType == typeof(int))
            {
                return Lucene.Net.Search.SortField.INT;
            }
            if (fieldType == typeof(long))
            {
                return Lucene.Net.Search.SortField.LONG;
            }
            if (fieldType == typeof(string))
            {
                return Lucene.Net.Search.SortField.STRING;
            }
            if (fieldType == typeof(float))
            {
                return Lucene.Net.Search.SortField.FLOAT;
            }
            if (fieldType == typeof(double))
            {
                return Lucene.Net.Search.SortField.DOUBLE;
            }
            if (fieldType == typeof(decimal))
            {
                return Lucene.Net.Search.SortField.DOUBLE;
            }
            if (fieldType == typeof(DateTime))
            {
                return Lucene.Net.Search.SortField.LONG;
            }

            return Lucene.Net.Search.SortField.STRING;
        }
    }
}