using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.Settings.Services
{
    public interface ISettingService
    {
        string Get(string key);

        T Get<T>(string key);

        void Set(string key, object value);
    }

    public static class SettingServiceExtensions
    {
        public static T Get<T>(this ISettingService service)
        {
            return service.Get<T>(typeof(T).Name);
        }

        public static void Set<T>(this ISettingService service, T value)
        {
            service.Set(typeof(T).Name, value);
        }
    }
}
