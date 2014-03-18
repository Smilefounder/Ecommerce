using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Kooboo.Commerce;

namespace Kooboo.Commerce.Search
{
    public class LocaleDateTimeIndexPathBuilder : IIndexPathBuilder
    {
        public string[] GetAllPath(string directory)
        {
            DirectoryInfo dir = new DirectoryInfo(directory);
            var paths = dir.GetDirectories("*", SearchOption.AllDirectories);
            return paths.Where(o =>
                {
                    var dirs = o.GetDirectories();
                    return dirs == null || dirs.Length <= 0;
                }).Select(o => o.FullName).ToArray();
        }

        public string BuildIndexPath(IDictionary<string, object> values)
        {
            var locale = string.Empty;
            if (values.ContainsKey("Locale"))
                locale = values["Locale"].ToString();

            if (!string.IsNullOrEmpty(locale))
            {
                DateTime? createTime = null;
                if (values.ContainsKey("CreateTime"))
                {
                    createTime = DateTime.Parse(values["CreateTime"].ToString());
                    if (createTime.HasValue)
                    {
                        DateTime beginDate = new DateTime(createTime.Value.Year, createTime.Value.Month, 1);
                        DateTime endDate = beginDate.AddMonths(1).AddDays(-1.0d);
                        return string.Format("{1}{0}{2:yyyyMMdd}_{3:yyyyMMdd}", Path.DirectorySeparatorChar, locale, beginDate, endDate);
                    }
                }
                else
                {
                    return locale;
                }
            }
            return string.Empty;
        }

        public string[] BuildSearchPaths(IDictionary<string, object> values)
        {
            string dicLocales = values["Locales"].ToString();
            DateTime beginTime = (DateTime)values["BeginTime"];
            DateTime endTime = (DateTime)values["EndTime"];
            List<string> paths = new List<string>();
            string[] locales = dicLocales.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
            foreach (var locale in locales)
            {
                DateTime beginDate = new DateTime(beginTime.Year, beginTime.Month, 1);
                while (beginDate < endTime)
                {
                    DateTime endDate = beginDate.AddMonths(1).AddDays(-1.0d);
                    string p = string.Format("{1}{0}{2:yyyyMMdd}_{3:yyyyMMdd}", Path.DirectorySeparatorChar, locale, beginDate, endDate);
                    paths.Add(p);

                    beginDate = beginDate.AddMonths(1);
                }
            }
            return paths.ToArray();
        }
    }
}
