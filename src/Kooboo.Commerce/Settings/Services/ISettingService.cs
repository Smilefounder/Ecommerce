using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.Settings.Services
{
    public interface ISettingService
    {
        string Get(string key, string category = null);

        T Get<T>(string key, string category = null);

        void Set(string key, object value, string category = null);

        IEnumerable<KeyValueSetting> GetByCategory(string category);
    }
}
